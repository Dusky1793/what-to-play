using System.Xml.Serialization;

namespace WhatToPlay.API.Models.SteamApiResponse
{

    [XmlRoot(ElementName = "playerstats")]
    public class GetOldApiPlayerStatsResponse
    {
        [XmlElement(ElementName = "privacyState")]
        public string privacyState { get; set; }

        [XmlElement(ElementName = "visibilityState")]
        public string visibilityState { get; set; }

        [XmlElement(ElementName = "game")]
        public OldApiGame game { get; set; }

        [XmlElement(ElementName = "player")]
        public OldApiPlayer player { get; set; }

        [XmlElement(ElementName = "stats")]
        public OldApiStats stats { get; set; }

        [XmlElement(ElementName = "achievements")]
        public OldApiAchievements achievements { get; set; }
    }

    [XmlRoot(ElementName = "achievements")]
    public class OldApiAchievements
    {
        [XmlElement(ElementName = "achievement")]
        public List<OldApiAchievement> achievements;
    }

    [XmlRoot(ElementName = "game")]
    public class OldApiGame
    {
        [XmlElement(ElementName = "gameFriendlyName")]
        public string gameFriendlyName { get; set; }

        [XmlElement(ElementName = "gameName")]
        public string gameName { get; set; }

        [XmlElement(ElementName = "gameLink")]
        public string gameLink { get; set; }

        [XmlElement(ElementName = "gameIcon")]
        public string gameIcon { get; set; }

        [XmlElement(ElementName = "gameLogo")]
        public string gameLogo { get; set; }

        [XmlElement(ElementName = "gameLogoSmall")]
        public string gameLogoSmall { get; set; }
    }

    [XmlRoot(ElementName = "player")]
    public class OldApiPlayer
    {
        [XmlElement(ElementName = "steamID64")]
        public string steamID64 { get; set; }

        [XmlElement(ElementName = "customURL")]
        public string customURL { get; set; }
    }

    [XmlRoot(ElementName = "stats")]
    public class OldApiStats
    {
        [XmlElement(ElementName = "hoursPlayed")]
        public string hoursPlayed { get; set; }
    }

    [XmlRoot(ElementName = "achievement")]
    public class OldApiAchievement
    {
        [XmlElement(ElementName = "closed")]
        public bool closed { get; set; }

        [XmlElement(ElementName = "iconClosed")]
        public string iconClosed { get; set; }

        [XmlElement(ElementName = "iconOpen")]
        public string iconOpen { get; set; }

        [XmlElement(ElementName = "name")]
        public string name { get; set; }

        [XmlElement(ElementName = "apiname")]
        public string apiname { get; set; }

        [XmlElement(ElementName = "description")]
        public string description { get; set; }

        [XmlElement(ElementName = "unlockTimestamp")]
        public string unlockTimestamp { get; set; }
    }

}
