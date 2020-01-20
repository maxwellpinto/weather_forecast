using RabbitMQ.Client;

namespace weatherForecast.Services
{    
    public interface IMessageService
    {
        bool Enqueue(string message);
        IModel Channel();
    }
}