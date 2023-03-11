namespace WhatToPlay.API.Interfaces
{
    public interface IHttpClientService
    {
        TResponse SendRequest<TResponse>(string endPoint, string steamId, string[] extraParams = null);
    }
}
