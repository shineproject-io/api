using ListsAPI.Features.Profile.DataAccess;
using ListsAPI.Features.Profile.ResponseModels;
using ListsAPI.Infrastructure.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Search
{
    [ApiController]
    public class ProfilePictureController : ControllerBase
    {
        private readonly IUserProfileWriter _userProfileWriter;
        private readonly IUserProfileReader _userProfileReader;
        private readonly IAzureStorageManager _storeFile;

        public ProfilePictureController(IUserProfileWriter userProfileWriter, IUserProfileReader userProfileReader, IAzureStorageManager storeFile)
        {
            _userProfileWriter = userProfileWriter;
            _userProfileReader = userProfileReader;
            _storeFile = storeFile;
        }

        [HttpPut]
        [Authorize]
        [Route("api/userprofiles/me/profilepicture")]
        [ProducesResponseType(typeof(UserProfileResponse), 200)]
        public async Task<IActionResult> ChangeProfilePicture()
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var userProfile = await _userProfileReader.GetByUserProfileId(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (Request.Form.Files.Count == 0)
            {
                return BadRequest("You must attach an image");
            }

            var profilePicture = Request.Form.Files[0];

            if (profilePicture.Length > 3000000)
            {
                return StatusCode(413);
            }

            var acceptedContentTypes = new List<string> { "image/png", "image/jpg", "image/jpeg", "image/gif" };

            if (!acceptedContentTypes.Contains(profilePicture.ContentType))
            {
                return StatusCode(415);
            }

            var fileName = Guid.NewGuid().ToString();

            var existingProfilePictureFileName = userProfile.ProfilePictureFileName;

            const string CONTAINER_NAME = "profilepictures";

            var absoluteFilePath = await _storeFile.StoreFile(CONTAINER_NAME, fileName, profilePicture);

            await _userProfileWriter.SetProfilePicturePath(userProfile.Id, absoluteFilePath, fileName);

            if (!string.IsNullOrEmpty(existingProfilePictureFileName))
            {
                await _storeFile.DeleteFile(CONTAINER_NAME, existingProfilePictureFileName);
            }

            return NoContent();
        }
    }
}