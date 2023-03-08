using Microsoft.AspNetCore.Mvc;
using Weather.Api.Services.Contracts;

namespace Weather.Api.Controllers;

public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> GetWeatherForCity(string city)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(city);
        if (weather is null)
            return NotFound();

        return Ok(weather);
    }
}