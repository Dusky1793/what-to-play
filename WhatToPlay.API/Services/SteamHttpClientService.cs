using System.Linq;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models;

namespace WhatToPlay.API.Services
{
    public class SteamHttpClientService : IHttpClientService
    {
        private readonly IConfiguration _configuration;
        private readonly string STEAM_API_KEY;
        private readonly string STEAM_API_BASE_ADDRESS;
        private readonly string STEAM_OLD_API_BASE_ADDRESS;

        public SteamHttpClientService(IConfiguration configuration)
        {
            _configuration = configuration;

            if(_configuration == null)
            {
                throw new Exception("IConfiguration cannot be null.");
            }

            STEAM_API_KEY = _configuration["SteamApiKey"];
            STEAM_API_BASE_ADDRESS = _configuration["SteamApiBaseUrl"];
            STEAM_OLD_API_BASE_ADDRESS = _configuration["OldSteamApiBaseUrl"];
        }

        public Task<string> SendRequest(string endPoint, string steamId, RequestParamsOptions extraParamsOptions = null)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(STEAM_API_BASE_ADDRESS);

            var extraParamsSubUrl = BuildExtraParamsSubUrl(extraParamsOptions);

            var requestUrl = $"{STEAM_API_BASE_ADDRESS}{endPoint}?key={STEAM_API_KEY}&steamid={steamId}{extraParamsSubUrl}";

            var response = httpClient.GetStringAsync(requestUrl);

            return response;
        }

        public Task<string> SendOldApiRequest(string endPoint, RequestParamsOptions extraParamsOptions = null)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(STEAM_OLD_API_BASE_ADDRESS);

            var extraParamsSubUrl = BuildExtraParamsSubUrl(extraParamsOptions);

            var requestUrl = $"{STEAM_OLD_API_BASE_ADDRESS}{endPoint}{extraParamsSubUrl}";

            var response = httpClient.GetStringAsync(requestUrl);

            return response;
        }

        private string BuildExtraParamsSubUrl(RequestParamsOptions requestParamsOptions)
        {
            if (requestParamsOptions == null || requestParamsOptions.ExtraParams == null || requestParamsOptions.ExtraParams.Length <= 0)
            {
                return string.Empty;
            }

            return $"&{String.Join(requestParamsOptions.Delimeter, requestParamsOptions.ExtraParams)}";
        }
    }
}
