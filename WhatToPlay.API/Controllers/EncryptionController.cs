using Microsoft.AspNetCore.Mvc;
using WhatToPlay.API.Interfaces;

namespace WhatToPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Encryptioncontroller : Controller
    {
        private readonly ILogger<SteamController> _logger;
        private readonly IEncryptionService _encryptionService;

        public Encryptioncontroller(ILogger<SteamController> logger, IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _logger = logger;
        }

        [HttpPost(Name = "EncryptSteamId")]
        public string EncryptSteamId([FromBody] string steamId)
        {
            return _encryptionService.Encrypt(steamId);
        }

         //this should never be exposed, except for testing purposes
        //[HttpPost(Name = "DecryptSteamId")]
        //public string DecryptSteamId(string encryptedSteamId)
        //{
        //    return _encryptionService.Decrypt(encryptedSteamId); // return as an ecrypted value
        //}
    }
}
