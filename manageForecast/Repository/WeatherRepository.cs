using manageForecast.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace manageForecast.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly ManagerWeatherContext _context;

        public WeatherRepository(ManagerWeatherContext context) 
        {
            this._context = context;
        }


        public void AddWeather(WeatherForecast model)
        {
            this._context.Add(model);
            this._context.SaveChanges();
        }

        public IList<WeatherForecast> GetAll()
        {
            return this._context.Set<WeatherForecast>()
            .Include(x => x.coord)
            .Include(x => x.weather)
            .Include(x => x.main)
            .Include(x => x.wind)
            .Include(x => x.clouds)
            .Include(x => x.sys)            
            .ToList();
        }
    }
}
