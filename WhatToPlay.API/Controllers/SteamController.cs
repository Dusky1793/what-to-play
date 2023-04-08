using Microsoft.AspNetCore.Mvc;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models.ApiResponse;

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
                return _steamService.GetAllOwnedGamesBySteamIdWithRetry(encryptedSteamId);
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
                return _steamService.GetPartialAchievementDetailsByAppIdWithRetry(encryptedSteamId, appId);
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
                return _steamService.GetFullAchievementDetailsByAppIdWithRetry(encryptedSteamId, appId);
            }
            catch (Exception ex)
            {
                // TODO: implement logger
                return null;
            }
        }
    }
}
