﻿namespace CryptoWorld.News.Core.ViewModels
{
    public class LoginResponseModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}