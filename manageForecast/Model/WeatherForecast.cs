using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace manageForecast.Model
{


    public class Coord
    {
        [Key]
        public int IdDatabase { get; set; }
        public int id { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        [Key]
        public int IdDatabase { get; set; }
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        [Key]
        public int IdDatabase { get; set; }
        public int id { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        [Key]
        public int IdDatabase { get; set; }
        public int id { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        [Key]
        public int IdDatabase { get; set; }
        public int id { get; set; }
        public int all { get; set; }
    }

    public class Sys
    {
        [Key]
        public int IdDatabase { get; set; }
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class WeatherForecast
    {
        [Key]
        public int IdDatabase { get; set; }
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

    }
    
 
    

}
