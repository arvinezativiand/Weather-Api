﻿using CleanArch.Application.City;
using CleanArch.Application.Interfaces;
using CleanArch.Domain.CityAggregate;
using Newtonsoft.Json;

namespace CleanArch.Infrastructure.City;

public class CityClient : ICityClient
{
    public MinimalCity GetMinimalCity(CityInformation city) => new MinimalCity(city.lat, city.lon, city.name);

    public async Task<MinimalCity> GetCityInformation(HttpClient Client, string cityName)
    {
        var baseUri = Environment.GetEnvironmentVariable("BASE_CITY_URI");
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");

        var uri = $"{baseUri}?q={cityName}&appid={apiKey}";
        var response = await Client.GetStringAsync(uri);

        var Jrespone = JsonConvert.DeserializeObject<List<CityInformation>>(response);
        if (Jrespone == null)
        {
            throw new ArgumentNullException();
        }

        var city = Jrespone[0];
        return new MinimalCity(city.lat, city.lon, cityName);
    }
}
