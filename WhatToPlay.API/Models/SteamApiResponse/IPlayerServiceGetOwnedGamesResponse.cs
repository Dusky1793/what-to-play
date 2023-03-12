namespace WhatToPlay.API.Models.SteamApiResponse
{
    public class IPlayerServiceGetOwnedGamesResponse
    {
        public int Game_Count { get; set; }
        public IEnumerable<SteamGame> Games { get; set; }
    }

    public class SteamGame
    {
        public string appId { get; set; }
        public string name { get; set; }
        public string playtime_Forever { get; set; }
        public string playtime_Forever_Hours => $"{Math.Round((double.Parse(playtime_Forever) / 60), 0).ToString()} Hours";
        public string img_Icon_Url { get; set; }
        public string playtime_Windows_Forever { get; set; }
        public string playtime_Windows_Forever_Hours => $"{Math.Round((double.Parse(playtime_Windows_Forever) / 60), 0).ToString()} Hours";
        public string playtime_Mac_Forever { get; set; }
        public string playtime_Mac_Forever_Hours => $"{Math.Round((double.Parse(playtime_Mac_Forever) / 60), 0).ToString()} Hours";
        public string playtime_Linux_Forever { get; set; }
        public string playtime_Linux_Forever_Hours => $"{Math.Round((double.Parse(playtime_Linux_Forever) / 60), 0).ToString()} Hours";
        public string rtime_Last_Played { get; set; }
        public DateTime rtime_Last_Played_DateTime => DateTimeOffset.FromUnixTimeSeconds(long.Parse(rtime_Last_Played)).DateTime;
    }
}
