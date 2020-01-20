using System.Threading.Tasks;

namespace weatherForecast.Services
{    
    public interface IWeatherForecastService
    {
        Task<string> GetOpenweatherForecast(string city);
    }
}