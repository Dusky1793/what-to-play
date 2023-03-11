namespace WhatToPlay.API.Models.SteamApiResponse
{
    public class ISteamUserStatsGetPlayerAchievementsResponse
    {
        public PlayerStats playerStats { get; set; }
    }

    public class PlayerStats
    {
        public string steamId { get; set; }
        public string gameName { get; set; }
        public IEnumerable<Achievement> achievements { get; set; }
    }

    public class Achievement
    {
        public string apiName { get; set; }
        public bool achieved { get; set; }
        public string unlocktime { get; set; }
    }
}
