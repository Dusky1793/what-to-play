namespace WhatToPlay.API.Interfaces
{
    public interface IHttpClientService
    {
        Task<string> SendRequest(string endPoint, string steamId, string[] extraParams = null);
        Task<string> SendOldApiRequest(string url, string[] extraParams = null);
    }
}
