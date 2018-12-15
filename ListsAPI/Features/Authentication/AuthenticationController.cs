using ListsAPI.Features.Authentication.Helpers;
using ListsAPI.Features.Authentication.RequestModels;
using ListsAPI.Features.Authentication.ResponseModels;
using ListsAPI.Features.Profile.DataAccess;
using ListsAPI.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ListsAPI.Features.Authentication
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserProfileReader _userProfileReader;
        private readonly IAuthenticationTokenProvider _authenticationTokenProvider;

        public AuthenticationController(IUserProfileReader userProfileReader, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            _userProfileReader = userProfileReader;
            _authenticationTokenProvider = authenticationTokenProvider;
        }

        /// <summary>
        /// Sign in and request an authentication token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/authentication/signin")]
        [ProducesResponseType(typeof(SignInResponse), 200)]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var userProfile = await _userProfileReader.GetByEmailAddress(request.EmailAddress);

            if (userProfile == null)
            {
                return NotFound();
            }

            var passwordValid = PasswordHasher.CompareSecurePassword(request.Password, userProfile.Password);

            if (!passwordValid)
            {
                return BadRequest("Invalid credentials");
            }

            var authenticationToken = _authenticationTokenProvider.Generate(request.EmailAddress, userProfile.Id.ToString());

            var response = new SignInResponse
            {
                Token = authenticationToken.Token,
                Expiration = authenticationToken.ExpirationDateTime
            };

            return Ok(response);
        }
    }
}