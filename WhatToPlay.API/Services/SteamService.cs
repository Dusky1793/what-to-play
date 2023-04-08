﻿using Newtonsoft.Json;
using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Models.ApiResponse;
using WhatToPlay.API.Models;
using WhatToPlay.API.Models.SteamApiResponse;
using System.Xml.Serialization;

namespace WhatToPlay.API.Services
{
    public class SteamService : ISteamService
    {
        private IHttpClientService _httpClientService;
        private IEncryptionService _encryptionService;

        public SteamService(IHttpClientService httpClientService, IEncryptionService encryption)
        {
            _httpClientService = httpClientService;
            _encryptionService = encryption;
        }

        public GetAllOwnedGamesResponse GetAllOwnedGamesBySteamId(string encryptedSteamId)
        {
            var steamId = _encryptionService.Decrypt(encryptedSteamId);

            var result = _httpClientService.SendRequest("IPlayerService/GetOwnedGames/v0001/", steamId, new RequestParamsOptions
            {
                Delimeter = "&",
                ExtraParams = new string[]
                {
                    "include_appinfo=true",
                    "include_extended_appinfo=true"
                }
            });

            result.Wait();

            var response = JsonConvert.DeserializeObject<GetAllOwnedGamesResponse>(result.Result);

            return response;
        }

        public GetAchievementDetailsByAppIdResponse GetPartialAchievementDetailsByAppId(string encryptedSteamId, string appId)
        {
            var steamId = _encryptionService.Decrypt(encryptedSteamId);

            var resultNewApi = _httpClientService.SendRequest("ISteamUserStats/GetPlayerAchievements/v0001/", steamId, new RequestParamsOptions
            {
                Delimeter = "&",
                ExtraParams = new string[]
                {
                    $"appid={appId}"
                }
            });

            try
            {
                Task.WaitAll(resultNewApi);
            }
            catch (HttpRequestException ex) { }
            catch (Exception ex) { }

            string newApiResult = string.Empty;
            if (resultNewApi.IsCompletedSuccessfully)
            {
                newApiResult = resultNewApi.Result;
            }

            // deserialize new api response (json)
            var newApiResponse = JsonConvert.DeserializeObject<ISteamUserStatsGetPlayerAchievementsResponse>(newApiResult);

            // left join details from the new and old api responses to form the final response
            var response = new GetAchievementDetailsByAppIdResponse
            {
                gameName = newApiResponse?.playerStats?.gameName,
                IsNewApiSuccessful = resultNewApi.IsCompletedSuccessfully
            };

            response.achievements = new List<Models.ApiResponse.Achievement>();

            if (response.IsNewApiSuccessful && newApiResponse != null && newApiResponse.playerStats != null && newApiResponse.playerStats.achievements != null)
            {
                response.achievements = newApiResponse.playerStats.achievements.Select(ac => new Models.ApiResponse.Achievement
                {
                    achieved = ac.achieved,
                    apiName = ac.apiName,
                    unlocktime = ac.unlocktime
                }).ToList();
            }

            return response;
        }

        public GetAchievementDetailsByAppIdResponse GetFullAchievementDetailsByAppId(string encryptedSteamId, string appId)
        {
            var steamId = _encryptionService.Decrypt(encryptedSteamId);

            var resultNewApi = _httpClientService.SendRequest("ISteamUserStats/GetPlayerAchievements/v0001/", steamId, new RequestParamsOptions
            {
                Delimeter = "&",
                ExtraParams = new string[]
                {
                    $"appid={appId}"
                }
            });

            var resultOldApi = _httpClientService.SendOldApiRequest($"profiles/{steamId}/stats/{appId}/?xml=1");

            try
            {
                Task.WaitAll(resultNewApi, resultOldApi);
            }
            catch (HttpRequestException ex) { }
            catch (Exception ex) { }

            // implement a re-try for the old-api

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
                    catch (Exception ex) { } // suppress parse exception, sometimes steam would fail returing data inconsistently
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

            if (response.IsNewApiSuccessful && newApiResponse.playerStats != null && newApiResponse.playerStats.achievements != null)
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
                }).OrderByDescending(ac => ac.unlocktime_DateTime).ToList();
            }

            return response;
        }
    }
}
