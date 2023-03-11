using System.Xml.Serialization;
using WhatToPlay.API.Models.SteamApiResponse;

namespace WhatToPlay.API.Models.ApiResponse
{
    public class GetAchievementDetailsByAppIdResponse
    {
        public string privacyState { get; set; }
        public string visibilityState { get; set; }
        public string gameName { get; set; }
        public string gameLink { get; set; }

        public string gameIcon { get; set; }
        public string gameLogo { get; set; }
        public string gameLogoSmall { get; set; }
        public List<Achievement> achievements { get; set; }
    }

    public class Achievement
    {
        public string apiName { get; set; }
        public bool achieved { get; set; }
        public string unlocktime { get; set; }

        public bool? achieved_OldApi { get; set; }
        public string iconClosed { get; set; }
        public string iconOpen { get; set; }
        public string name { get; set; }
        public string apiname_OldApi { get; set; }
        public string description { get; set; }
    }
}
