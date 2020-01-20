using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weatherForecast.Services;

namespace weatherForecast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        private readonly IMessageService _messageService;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController( IMessageService messageService, IWeatherForecastService weatherForecastService )
        {            
            this._messageService = messageService;    
            this._weatherForecastService = weatherForecastService;        
        }
        
        // GET: api/city
        [HttpGet("{city}/{country}")]  
        public async Task GetAsync(string city, string country)
        {
            string query = $"{city},{country}";
            string resultContent = await _weatherForecastService.GetOpenweatherForecast(query);

            if (_messageService.Enqueue(resultContent))
            {
                Console.WriteLine($"result was enqueued: {resultContent} ");
            }
        }
    }
}
