using WhatToPlay.API.Models;

namespace WhatToPlay.API.Interfaces
{
    public interface IHttpClientService
    {
        Task<string> SendRequest(string endPoint, string steamId, RequestParamsOptions extraParamsOptions = null);
        Task<string> SendOldApiRequest(string url, RequestParamsOptions extraParamsOptions = null);
    }
}
