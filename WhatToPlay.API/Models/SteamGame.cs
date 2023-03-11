namespace WhatToPlay.API.Models
{
    public class SteamGame
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public string Playtime_Forever { get; set; }
        public string Playtime_Forever_Hours => $"{(double.Parse(Playtime_Forever) / 60).ToString()} Hours";
        public string Img_Icon_Url { get; set; }
        public string Playtime_Windows_Forever { get; set; }
        public string Playtime_Windows_Forever_Hours => $"{(double.Parse(Playtime_Windows_Forever) / 60).ToString()} Hours";
        public string Playtime_Mac_Forever { get; set; }
        public string Playtime_Mac_Forever_Hours => $"{(double.Parse(Playtime_Mac_Forever) / 60).ToString()} Hours";
        public string Playtime_Linux_Forever { get; set; }
        public string Playtime_Linux_Forever_Hours => $"{(double.Parse(Playtime_Linux_Forever) / 60).ToString()} Hours";
        public string Rtime_Last_Played { get; set; }
        public DateTime Rtime_Last_Played_DateTime => DateTimeOffset.FromUnixTimeSeconds(long.Parse(Rtime_Last_Played)).DateTime;
    }
}
