using System;
using System.Security.Cryptography;

namespace ListsAPI.Features.Authentication.Helpers
{
    internal static class PasswordHasher
    {
        internal static string GenerateSecurePassword(string password)
        {
            // http://stackoverflow.com/questions/4181198/how-to-hash-a-password
            using (var rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var salt = new byte[16];
                rNGCryptoServiceProvider.GetBytes(salt);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    var hash = pbkdf2.GetBytes(20);
                    var hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    var savedPasswordHash = Convert.ToBase64String(hashBytes);
                    return savedPasswordHash;
                }
            }
        }

        internal static bool CompareSecurePassword(string passwordAttempt, string passwordActual)
        {
            var savedPasswordHash = passwordActual;
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordAttempt, salt, 10000))
            {
                var hash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}