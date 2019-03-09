using ListsAPI.Features.Profile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile
{
    [ApiController]
    public class UserProfileDeletionController : Controller
    {
        private readonly IUserProfileDeletionManager _userProfileDeletionManager;

        public UserProfileDeletionController(IUserProfileDeletionManager userProfileDeletionManager)
        {
            _userProfileDeletionManager = userProfileDeletionManager;
        }

        /// <summary>
        /// Delete the user profile and all contents of the currently signed in user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/userprofiles/me/delete")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete()
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            await _userProfileDeletionManager.DeleteUserProfile(userProfileId);

            return Ok();
        }
    }
}