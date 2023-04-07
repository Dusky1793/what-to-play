using WhatToPlay.API.Models.ApiResponse;

namespace WhatToPlay.API.Interfaces
{
    public interface ISteamService
    {
        GetAllOwnedGamesResponse GetAllOwnedGamesBySteamId(string encryptedSteamId);
        GetAchievementDetailsByAppIdResponse GetPartialAchievementDetailsByAppId(string encryptedSteamId, string appId);
        GetAchievementDetailsByAppIdResponse GetFullAchievementDetailsByAppId(string encryptedSteamId, string appId);
    }
}
