using System;
using System.Text;
using RabbitMQ.Client;
using manageForecast.Utils;

namespace manageForecast.Services
{
    public class MessageService : IMessageService
    {

        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;

        private bool InDocker { get { return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; } }

        public MessageService(RabbitMQConfigurations configurations)
        {
            Console.WriteLine("[ Manager ] connecting to rabbit");

            _factory = new ConnectionFactory()
            {
                HostName = (InDocker) ? configurations.HostNameDocker : configurations.HostName,
                Port = configurations.Port,
                UserName = configurations.UserName,
                Password = configurations.Password
            };

            if (InDocker)
            {
                _factory.HostName = configurations.HostNameDocker;
            }

            _conn = _factory.CreateConnection();

            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: Constants.RabbitQueuRequest,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            _channel.QueueDeclare(queue: Constants.RabbitQueuResponse,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

        }
        public bool Enqueue(string messageString)
        {
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish(exchange: "",
                                routingKey: Constants.RabbitQueuRequest,
                                basicProperties: null,
                                body: body);
            Console.WriteLine("[ Manager ] Event to Weather request was Published to RabbitMQ", messageString);
            return true;
        }

        public IModel Channel()
        {
            return _channel;
        }
    }
}