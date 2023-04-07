using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Serialization;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models;
using WhatToPlay.API.Models.ApiResponse;
using WhatToPlay.API.Models.SteamApiResponse;

namespace WhatToPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SteamController : Controller
    {
        private readonly ILogger<SteamController> _logger;
        private ISteamService _steamService;

        public SteamController(ILogger<SteamController> logger, ISteamService steamService)
        {
            _logger = logger;
            _steamService = steamService;
        }

        [HttpGet(Name = "GetAllOwnedGames")]
        public GetAllOwnedGamesResponse GetAllOwnedGames(string encryptedSteamId)
        {
            try
            {
                return _steamService.GetAllOwnedGamesBySteamId(encryptedSteamId);
            }
            catch(Exception ex)
            {
                // TODO: implement logger
                return null;
            }
        }

        [HttpGet("")]
        public GetAchievementDetailsByAppIdResponse GetPartialAchievementDetailsByAppId(string encryptedSteamId, string appId)
        {
            try
            {
                return _steamService.GetPartialAchievementDetailsByAppId(encryptedSteamId, appId);
            }
            catch (Exception ex)
            {
                // TODO: implement logger
                return null;
            }
        }

        [HttpGet(Name = "GetAchievementDetailsByAppId")]
        public GetAchievementDetailsByAppIdResponse GetFullAchievementDetailsByAppId(string encryptedSteamId, string appId)
        {
            try
            {
                return _steamService.GetFullAchievementDetailsByAppId(encryptedSteamId, appId);
            }
            catch (Exception ex)
            {
                // TODO: implement logger
                return null;
            }
        }
    }
}
