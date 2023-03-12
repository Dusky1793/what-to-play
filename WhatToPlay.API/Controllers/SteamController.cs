using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Serialization;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models.ApiResponse;
using WhatToPlay.API.Models.SteamApiResponse;

namespace WhatToPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SteamController : Controller
    {
        /*
         * TODO: migrate base data retrieval logic into a steam service class
         */

        private readonly ILogger<SteamController> _logger;
        private IHttpClientService _httpClientService;
        private IEncryptionService _encryptionService;

        public SteamController(ILogger<SteamController> logger, IHttpClientService httpClientService, IEncryptionService encryptionService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _encryptionService = encryptionService;
        }

        [HttpGet(Name = "GetAllOwnedGames")]
        public GetAllOwnedGamesResponse GetAllOwnedGames(string encryptedSteamId)
        {
            var steamId = _encryptionService.Decrypt(encryptedSteamId);

            var endPoint = "IPlayerService/GetOwnedGames/v0001/";
            var result = _httpClientService.SendRequest(endPoint, steamId, 
                new string[] 
                { 
                    "include_appinfo=true",
                    "include_extended_appinfo=true"
                });

            result.Wait();

            var response = JsonConvert.DeserializeObject<GetAllOwnedGamesResponse>(result.Result);

            return response;
        }

        [HttpGet(Name = "GetAchievementDetailsByAppId")]
        public GetAchievementDetailsByAppIdResponse GetAchievementDetailsByAppId(string encryptedSteamId, string appId)
        {
            var steamId = _encryptionService.Decrypt(encryptedSteamId);

            var endPoint = "ISteamUserStats/GetPlayerAchievements/v0001/";
            var resultNewApi = _httpClientService.SendRequest(endPoint, steamId,
                new string[]
                {
                    $"appid={appId}"
                });

            var requestUrl = $"http://steamcommunity.com/profiles/{steamId}/stats/{appId}/?xml=1";
            var resultOldApi = _httpClientService.SendOldApiRequest(requestUrl);

            try
            {
                Task.WaitAll(resultNewApi, resultOldApi);
            }
            catch(HttpRequestException ex) { }
            catch(Exception ex) { }

            string newApiResult = string.Empty;
            if (resultNewApi.IsCompletedSuccessfully)
            {
                newApiResult = resultNewApi.Result;
            }

            // deserialize old api response (xml)
            var xmlSerializer = new XmlSerializer(typeof(GetOldApiPlayerStatsResponse));

            var oldApiResponse = new GetOldApiPlayerStatsResponse()
            {
                achievements = new OldApiAchievements
                {
                    achievements = new List<OldApiAchievement>()
                }
            };
            if (resultOldApi.IsCompletedSuccessfully) // the old api doesn't give a BAD_REQUEST upon failure
            {
                var oldApiResult = resultOldApi.Result;
                using (var reader = new StringReader(oldApiResult))
                {
                    try
                    {
                        oldApiResponse = (GetOldApiPlayerStatsResponse)xmlSerializer.Deserialize(reader);
                    }
                    catch(Exception ex) { } // suppress parse exception, sometimes steam would fail returing data inconsistently
                }
            }

            // deserialize new api response (json)
            var newApiResponse = JsonConvert.DeserializeObject<ISteamUserStatsGetPlayerAchievementsResponse>(newApiResult);

            // left join details from the new and old api responses to form the final response
            var response = new GetAchievementDetailsByAppIdResponse
            {
                gameName = newApiResponse?.playerStats?.gameName,
                gameIcon = oldApiResponse?.game?.gameIcon,
                gameLink = oldApiResponse?.game?.gameLink,
                gameLogo = oldApiResponse?.game?.gameLogo,
                gameLogoSmall = oldApiResponse?.game?.gameLogoSmall,
                privacyState = oldApiResponse?.privacyState,
                visibilityState = oldApiResponse?.visibilityState,
                IsNewApiSuccessful = resultNewApi.IsCompletedSuccessfully,
                IsOldApiSuccessful = oldApiResponse.player != null && oldApiResponse.game != null
            };

            response.achievements = new List<Models.ApiResponse.Achievement>();

            if(response.IsNewApiSuccessful && newApiResponse.playerStats != null && newApiResponse.playerStats.achievements != null)
            {
                response.achievements = newApiResponse.playerStats.achievements
                .GroupJoin(
                oldApiResponse.achievements.achievements,
                    newApiAchievement => newApiAchievement.apiName.ToLower(),
                    oldApiAchievement => (oldApiAchievement != null ? oldApiAchievement.apiname.ToLower() : ""),
                    (newApiAchv, oldApiAchv) => new
                    {
                        newApiAchievement = newApiAchv,
                        oldApiAchievement = oldApiAchv.SingleOrDefault()
                    }
                ).Select(grp => new Models.ApiResponse.Achievement
                {
                    achieved = grp.newApiAchievement.achieved,
                    apiName = grp.newApiAchievement.apiName,
                    unlocktime = grp.newApiAchievement.unlocktime,

                    achieved_OldApi = grp.oldApiAchievement?.closed,
                    apiname_OldApi = grp.oldApiAchievement?.apiname,
                    description = grp.oldApiAchievement?.description,
                    iconClosed = grp.oldApiAchievement?.iconClosed,
                    iconOpen = grp.oldApiAchievement?.iconOpen,
                    name = grp.oldApiAchievement?.name
                }).ToList();
            }

            return response;
        }
    }
}
