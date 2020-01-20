using System;
using System.Collections.Generic;
using manageForecast.Model;
using manageForecast.Repository;
using manageForecast.Services;
using Microsoft.AspNetCore.Mvc;

namespace manageForecast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherManagerController : ControllerBase
    {        
        private readonly IMessageService _messageService;
        private readonly WeatherRepository _repository;
        public WeatherManagerController(IMessageService messageService, WeatherRepository repository)
        {            
            _messageService = messageService;
            _repository = repository;
        }

        // GET: api/city/country
        [HttpGet("{city}/{country}")]  
        public void Get(string city, string country)
        {
            string query = $"{city},{country}";

            if (_messageService.Enqueue(query))
            {
                Console.WriteLine($"[ Manager ]  Weater Forecast was enqueued: {query} ");
            }
        }


        
        [HttpGet]     
        [Route("All")]   
        public IList<WeatherForecast> All()
        {
            return _repository.GetAll();
        }

    }
}
