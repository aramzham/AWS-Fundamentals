using Weather.Api.Models;

namespace Weather.Api.Services.Contracts;

public interface IWeatherService
{
    Task<WeatherResponse?> GetCurrentWeatherAsync(string city);
}