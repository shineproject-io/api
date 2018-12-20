using ListsAPI.Features.Authentication.Helpers;
using ListsAPI.Features.Profile.DataAccess;
using ListsAPI.Features.Profile.RequestModels;
using ListsAPI.Features.Profile.ResponseModels;
using ListsAPI.Infrastructure.ContentDelivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Search
{
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileWriter _userProfileWriter;
        private readonly IUserProfileReader _userProfileReader;
        private readonly IContentDeliveryNetworkResolver _cdnResolver;

        public UserProfilesController(IUserProfileWriter userProfileWriter, IUserProfileReader userProfileReader, IContentDeliveryNetworkResolver cdnResolver)
        {
            _userProfileWriter = userProfileWriter;
            _userProfileReader = userProfileReader;
            _cdnResolver = cdnResolver;
        }

        /// <summary>
        /// Get the user profile of the authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/userprofiles/me")]
        [ProducesResponseType(typeof(UserProfileResponse), 200)]
        public async Task<IActionResult> Get()
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            var userProfile = await _userProfileReader.GetByUserProfileId(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            var response = new UserProfileResponse
            {
                Id = userProfile.Id,
                EmailAddress = userProfile.EmailAddress,
                GivenName = userProfile.GivenName,
                FamilyName = userProfile.FamilyName,
                ProfilePicturePath = _cdnResolver.Resolve(userProfile.ProfilePicturePath)
            };

            return Ok(response);
        }

        /// <summary>
        /// Create a new User Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/userprofiles")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> Create(CreateUserProfileRequest request)
        {
            var existingUserProfile = await _userProfileReader.GetByEmailAddress(request.EmailAddress);

            if (existingUserProfile != null)
            {
                return BadRequest("Email address already in use");
            }

            var userProfileId = await _userProfileWriter.Add(request.EmailAddress, PasswordHasher.GenerateSecurePassword(request.Password), request.GivenName, request.FamilyName, request.ProfilePicturePath);

            return Ok(userProfileId);
        }

        /// <summary>
        /// Set the name of the authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/userprofiles/me/name")]
        public async Task<IActionResult> SetName(ChangeUserProfileNameRequest request)
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var userProfile = await _userProfileReader.GetByUserProfileId(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            await _userProfileWriter.SetName(userProfile.Id, request.GivenName, request.FamilyName);

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/userprofiles/me/password")]
        public async Task<IActionResult> SetPassword(ChangePasswordRequest request)
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var userProfile = await _userProfileReader.GetByUserProfileId(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (PasswordHasher.CompareSecurePassword(request.NewPassword, userProfile.Password))
            {
                return BadRequest("The current password supplied is incorrect");
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest("The password must be at least six characters");
            }

            var newPasswordHash = PasswordHasher.GenerateSecurePassword(request.NewPassword);

            await _userProfileWriter.SetPassword(userProfile.Id, newPasswordHash);

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/userprofiles/me/emailaddress")]
        public async Task<IActionResult> SetEmailAddress(ChangeEmailAddressRequest request)
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var userProfile = await _userProfileReader.GetByUserProfileId(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            var existingUserProfile = await _userProfileReader.GetByEmailAddress(request.EmailAddress);

            if (existingUserProfile != null && existingUserProfile.Id != userProfile.Id)
            {
                return BadRequest("Email address already in use");
            }

            // VALIDATE: Email Address Format
            // CONSIDER: Token Acceptance
            // VALIDATE: Not the current email address

            await _userProfileWriter.SetEmailAddress(userProfile.Id, request.EmailAddress);

            return Ok();
        }
    }
}