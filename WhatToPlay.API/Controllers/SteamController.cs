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

        public SteamController(ILogger<SteamController> logger, IHttpClientService httpClientService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
        }

        [HttpGet(Name = "GetAllOwnedGames")]
        public GetAllOwnedGamesResponse GetAllOwnedGames(string steamId)
        {
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
        public GetAchievementDetailsByAppIdResponse GetAchievementDetailsByAppId()//string steamId, string appId)
        {
            var steamId = "76561198865252681";
            var appId = "1446780";

            var endPoint = "ISteamUserStats/GetPlayerAchievements/v0001/";
            var resultNewApi = _httpClientService.SendRequest(endPoint, steamId,
                new string[]
                {
                    $"appid={appId}"
                });

            var requestUrl = $"http://steamcommunity.com/profiles/{steamId}/stats/{appId}/?xml=1";
            var resultOldApi = _httpClientService.SendOldApiRequest(requestUrl);

            Task.WaitAll(resultNewApi, resultOldApi);

            var oldApiResult = resultOldApi.Result;

            // deserialize old api response (xml)
            var xmlSerializer = new XmlSerializer(typeof(GetOldApiPlayerStatsResponse));

            var oldApiResponse = new GetOldApiPlayerStatsResponse();
            using (var reader = new StringReader(oldApiResult))
            {
                oldApiResponse = (GetOldApiPlayerStatsResponse)xmlSerializer.Deserialize(reader);
            }

            // deserialize new api response (json)
            var newApiResponse = JsonConvert.DeserializeObject<ISteamUserStatsGetPlayerAchievementsResponse>(resultNewApi.Result);

            // left join details from the new and old api responses to form the final response
            var response = new GetAchievementDetailsByAppIdResponse
            {
                gameName = newApiResponse?.playerStats?.gameName,
                gameIcon = oldApiResponse?.game?.gameIcon,
                gameLink = oldApiResponse?.game?.gameLink,
                gameLogo = oldApiResponse?.game?.gameLogo,
                gameLogoSmall = oldApiResponse?.game?.gameLogoSmall,
                privacyState = oldApiResponse?.privacyState,
                visibilityState = oldApiResponse?.visibilityState
            };

            response.achievements = newApiResponse.playerStats.achievements
                .GroupJoin(
                oldApiResponse.achievements.achievements,
                    newApiAchievement => newApiAchievement.apiName.ToLower(),
                    oldApiAchievement => oldApiAchievement.apiname.ToLower(),
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
                    name = grp.oldApiAchievement?.name,
                }).ToList();


            return response;
        }
    }
}
