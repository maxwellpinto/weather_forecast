using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using weatherForecast.Services;
using weatherForecast.Utils;

namespace weatherForecast
{
    public class Program
    {

        private static IConfiguration _configuration;
        private static MessageService messageService;

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
            messageService = new MessageService(rabbitMQConfigurations);

            //Create a Consumer to listening weatherForecastResquest queue.
            CreateConsumer(messageService.Channel());            
            CreateHostBuilder(args).Build().Run();
        }

        private static void CreateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_ReceivedAsync;
            channel.BasicConsume(queue: Constants.RabbitQueuRequest,
                                    autoAck: true,
                                    consumer: consumer);        
        }

        private static void Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [Weater Forecast] Requets to Weather was received from Rabbit: {0}", message);

            if(!string.IsNullOrEmpty(message))
            {
                var response = new WeatherForecastService().GetOpenweatherForecast(message);                                
                messageService.Enqueue(response.Result);
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
