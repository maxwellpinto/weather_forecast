using RabbitMQ.Client;

namespace manageForecast.Services
{    
    public interface IMessageService
    {
        bool Enqueue(string message);
        IModel Channel();
    }
}