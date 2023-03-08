﻿using System.Net;
using Microsoft.Extensions.Options;
using Weather.Api.Models;
using Weather.Api.Services.Contracts;

namespace Weather.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<OpenWeatherApiSettings> _weatherApiOptions;
    // IOptionsMonitor will monitor config file for changes

    public WeatherService(IHttpClientFactory httpClientFactory, IOptionsMonitor<OpenWeatherApiSettings> weatherApiOptions)
    {
        _httpClientFactory = httpClientFactory;
        _weatherApiOptions = weatherApiOptions;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_weatherApiOptions.CurrentValue.ApiKey}";
        var httpClient = _httpClientFactory.CreateClient();

        var weatherResponse = await httpClient.GetAsync(url);
        if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
            return null;

        var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
        return weather;
    }
}