using Microsoft.AspNetCore.Mvc;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models;
using WhatToPlay.API.Services;

namespace WhatToPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : Controller
    {
        private readonly ILogger<SteamController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string STEAM_API_KEY;
        private readonly IHttpClientService _httpClientService;

        public SteamController(ILogger<SteamController> logger, IConfiguration configuration, IHttpClientService httpClientService)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientService = httpClientService;

            STEAM_API_KEY = configuration["SteamApiKey"];
        }

        [HttpGet(Name = "GetAllOwnedGames")]
        public GetOwnedGamesResponse GetAllOwnedGames(string steamId)
        {
            var endPoint = "IPlayerService/GetOwnedGames/v0001/";
            var result = _httpClientService.SendRequest<GetOwnedGamesResponse>(endPoint, steamId, new string[] { "include_appinfo=true" });

            result.Response.Games = result.Response.Games.OrderByDescending(g => int.Parse(g.Playtime_Forever));

            return result;
        }
    }
}
