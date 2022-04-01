using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Repositorios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepositorio repositorio;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IRepositorio repositorio)
        {
            _logger = logger;
            this.repositorio = repositorio;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("guid")]
        public Guid ObtenerGUIDWeatherForecastController()
        {
            return repositorio.ObtenerGUID();
        }

    }
}