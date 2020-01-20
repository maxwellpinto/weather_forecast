using Microsoft.EntityFrameworkCore;

namespace manageForecast.Model
{
    public class ManagerWeatherContext : DbContext
    {
        private static bool _created = false;
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public ManagerWeatherContext(DbContextOptions<ManagerWeatherContext> options)
        : base(options)
        {
            CreateDataBase();
        }

        public ManagerWeatherContext()
        {
            CreateDataBase();
        }

       public void CreateDataBase()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WeatherForecast>().ToTable("WeatherForecast");
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=Weather.db");


    }


}
