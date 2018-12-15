using ListsAPI.Features.Authentication.Helpers;
using ListsAPI.Features.Profile.DataAccess;
using ListsAPI.Features.Profile.RequestModels;
using ListsAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile
{
    [ApiController]
    [AllowAnonymous]
    public class PasswordResetController : ControllerBase
    {
        private readonly IUserProfileWriter _userProfileWriter;
        private readonly IUserProfileReader _userProfileReader;
        private readonly IUserProfileTokenWriter _tokenWriter;
        private readonly IUserProfileTokenReader _tokenReader;
        private readonly IEmailMessenger _emailMessenger;

        public PasswordResetController(IUserProfileWriter userProfileWriter, IUserProfileReader userProfileReader, IUserProfileTokenWriter tokenWriter, IUserProfileTokenReader tokenReader, IEmailMessenger emailMessenger)
        {
            _userProfileWriter = userProfileWriter;
            _userProfileReader = userProfileReader;
            _tokenWriter = tokenWriter;
            _tokenReader = tokenReader;
            _emailMessenger = emailMessenger;
        }

        [HttpPost]
        [Route("api/userprofiles/requesttoken")]
        public async Task<IActionResult> RequestToken(PasswordResetTokenRequest request)
        {
            var userProfile = await _userProfileReader.GetByEmailAddress(request.EmailAddress);

            if (userProfile == null)
            {
                await _emailMessenger.SendAsync(request.EmailAddress, "Shine: Failed Password Reset Request", "Hello from Shine, somebody tried to request a password reset for an account held by your email address, however we do not have an account on record for you. If you attempted the request, please try an alternative email address or create an account. If you didn't attempt the reqest, don't worry - we don't have any data relating to this email address");
            }
            else
            {
                var currentDateTime = DateTime.UtcNow;

                var token = await _tokenWriter.Add(new Tables.UserProfileToken
                {
                    UserProfileId = userProfile.Id,
                    TokenType = Enums.UserProfileTokenType.PasswordReset,
                    Token = Guid.NewGuid().ToString(),
                    ExpirationTime = currentDateTime.AddDays(365),
                    DateCreated = currentDateTime,
                });

                await _emailMessenger.SendAsync(userProfile.EmailAddress, "Shine: Password Reset", $"Hello from Shine, you told us that you wanted to reset the password for your account, click here to change your password, {token}");
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/userprofiles/changepassword")]
        public async Task<IActionResult> ChangePassword(ResetPasswordRequest request)
        {
            var token = await _tokenReader.Get(request.Token, Enums.UserProfileTokenType.PasswordReset);

            if (token == null || token.DateUsed.HasValue || token.ExpirationTime < DateTime.UtcNow)
            {
                return NotFound();
            }

            var userProfile = await _userProfileReader.GetByEmailAddress(request.EmailAddress);

            if (userProfile == null)
            {
                return BadRequest("Unable to identify profile");
            }

            if (token.UserProfileId != userProfile.Id)
            {
                return BadRequest("Unable to change password for this profile");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Please enter a valid password");
            }

            await _tokenWriter.Use(token);

            await _userProfileWriter.SetPassword(userProfile.Id, PasswordHasher.GenerateSecurePassword(request.Password));

            return Ok();
        }
    }
}