using System;

namespace ListsAPI.Infrastructure.Authentication.Models
{
    public class AuthenticationToken
    {
        public string Token { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}