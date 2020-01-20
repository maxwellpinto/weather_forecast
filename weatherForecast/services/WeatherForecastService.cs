using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace weatherForecast.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {

        public async Task<string> GetOpenweatherForecast(string query)
        {
            string apiWeather = $"http://api.openweathermap.org/data/2.5/weather?q={query}&APPID=eb8b1a9405e659b2ffc78f0a520b1a46";
            Console.WriteLine($"[Weater Forecast] Retrieving city weather forecast : {query}");

            string resultContent = String.Empty;

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    var result = await client.GetAsync(apiWeather);
                    resultContent = await result.Content.ReadAsStringAsync();
                }
            }

            return resultContent;

        }

    }
}