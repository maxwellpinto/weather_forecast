using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace clientWeatherForecast
{
    class Program
    {
        private static string[] Citys => new string[] { "Blumenau,br", "Brasilia,br", "Sao Paulo,br", "rio de janeiro,br", "curitiba,br" };        

        private static bool InDocker { get { return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; } }

        static void Main(string[] args)
        {        

            Task.Delay(1000).Wait();

            Console.WriteLine("Posting messages to webApi");
            for (int i = 0; i < 5; i++)
            {
                PostMessage(Citys[i]).Wait();
            }

        }

        public static async Task PostMessage(string postData)
        {
            var json = JsonConvert.SerializeObject(postData);
            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var queryParamns = postData.Split(',');
            var urlClient = string.Empty;
            
            urlClient = $"http://localhost:10005/api/WeatherManager/{queryParamns[0]}/{queryParamns[1]}";
                        
            Console.WriteLine($"Post url {urlClient}");

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    
                    var result = await client.GetAsync( urlClient );

                    string resultContent = await result.Content.ReadAsStringAsync();

                    if(result.StatusCode == System.Net.HttpStatusCode.OK){
                        Console.WriteLine("Weather was required : " + resultContent);
                    }
                                        
                }
            }

        }

    }
}
