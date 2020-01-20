using manageForecast.Model;
using System.Collections.Generic;

namespace manageForecast.Repository
{
    public interface IWeatherRepository
    {
        void AddWeather(WeatherForecast model);
        IList<WeatherForecast> GetAll();
    }
}
