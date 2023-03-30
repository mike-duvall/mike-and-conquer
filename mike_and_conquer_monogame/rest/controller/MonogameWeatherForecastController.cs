using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using mike_and_conquer_monogame.rest.domain;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("[controller]")]
    public class MonogameWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // private readonly ILogger<MonogameWeatherForecastController> _logger;
        //
        // public MonogameWeatherForecastController(ILogger<MonogameWeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpGet]
        public IEnumerable<MonogameWeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new MonogameWeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
