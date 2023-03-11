﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WhatToPlay.API.Interfaces;

namespace WhatToPlay.API.Services
{
    public class SteamHttpClientService : IHttpClientService
    {
        private readonly IConfiguration _configuration;
        private readonly string STEAM_API_KEY;

        public SteamHttpClientService(IConfiguration configuration)
        {
            _configuration = configuration;
            STEAM_API_KEY = _configuration["SteamApiKey"];
        }

        public TResponse SendRequest<TResponse>(string endPoint, string steamId, string[] extraParams = null)
        {
            var baseAddress = _configuration["SteamApiBaseUrl"];
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            var requestUrl = $"http://api.steampowered.com/{endPoint}?key={STEAM_API_KEY}&steamid={steamId}=json{(extraParams != null && extraParams.Length > 0 ? $"&{String.Join("&", extraParams)}" : string.Empty)}";
            
            var response = httpClient.GetStringAsync(requestUrl);
            response.Wait();

            var result = JsonConvert.DeserializeObject<TResponse>(response.Result);

            return result;
        }
    }
}