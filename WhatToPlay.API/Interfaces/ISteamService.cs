using WhatToPlay.API.Models.ApiResponse;

namespace WhatToPlay.API.Interfaces
{
    public interface ISteamService
    {
        GetAllOwnedGamesResponse GetAllOwnedGamesBySteamId(string encryptedSteamId);
        GetAllOwnedGamesResponse GetAllOwnedGamesBySteamIdWithRetry(string encryptedSteamId, int? maxRetry = null, int? retryDelayMs = null);
        GetAchievementDetailsByAppIdResponse GetPartialAchievementDetailsByAppId(string encryptedSteamId, string appId);
        GetAchievementDetailsByAppIdResponse GetPartialAchievementDetailsByAppIdWithRetry(string encryptedSteamId, string appId, int? maxRetry = null, int? retryDelayMs = null);
        GetAchievementDetailsByAppIdResponse GetFullAchievementDetailsByAppId(string encryptedSteamId, string appId);
        GetAchievementDetailsByAppIdResponse GetFullAchievementDetailsByAppIdWithRetry(string encryptedSteamId, string appId, int? maxRetry = null, int? retryDelayMs = null);
    }
}
