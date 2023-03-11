namespace WhatToPlay.API.Models
{
    public class SteamGames
    {
        public int Game_Count { get; set; }
        public IEnumerable<SteamGame> Games { get; set; }
    }
}
