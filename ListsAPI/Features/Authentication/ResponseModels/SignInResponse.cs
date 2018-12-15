using System;

namespace ListsAPI.Features.Authentication.ResponseModels
{
    public class SignInResponse
    {
        public string Token { get; set; }

        public DateTime? Expiration { get; set; }
    }
}