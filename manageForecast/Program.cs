using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using manageForecast.Model;
using manageForecast.Services;
using manageForecast.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace manageForecast
{
    public class Program
    {

        private static IConfiguration _configuration;

        public static void Main(string[] args)
        {

            //Wait RabbitMq
            Task.Delay(1000).Wait();
            Console.WriteLine("Wait RabbitMq");

            //Set configurations
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            _configuration = builder.Build();

            //Import configurations from appsettings.json
            var rabbitMQConfigurations = new RabbitMQConfigurations();
            _configuration.GetSection("RabbitMQConfigurations").Bind(rabbitMQConfigurations);


            // Start to Message Broker
            MessageService messageService = new MessageService(rabbitMQConfigurations);

            //Create a Consumer to listening weatherForecastResponse queue.
            CreateConsumer(messageService.Channel());
            CreateHostBuilder(args).Build().Run();
        }

        private static void CreateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: Constants.RabbitQueuResponse,
                                    autoAck: true,
                                    consumer: consumer);
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[ Manager ] Weather was received from Rabbit: {message}");

            var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(message);

            using (var db = new ManagerWeatherContext())
            {
                db.Add(weatherForecast);
                db.SaveChanges();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
