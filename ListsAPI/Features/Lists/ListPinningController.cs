using ListsAPI.Features.Lists.Authorizers;
using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.Lists.ResponseModels;
using ListsAPI.Infrastructure.ContentDelivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists
{
    [ApiController]
    [Authorize]
    public class ListPinningController : ControllerBase
    {
        private readonly IListAuthoriser _listAuthoriser;
        private readonly IListWriter _listWriter;
        private readonly IListReader _listReader;
        private readonly IContentDeliveryNetworkResolver _cdnResolver;

        private int _userProfileId => Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

        public ListPinningController(IListAuthoriser listAuthoriser, IListWriter listWriter, IListReader listReader, IContentDeliveryNetworkResolver cdnResolver)
        {
            _listAuthoriser = listAuthoriser;
            _listWriter = listWriter;
            _listReader = listReader;
            _cdnResolver = cdnResolver;
        }

        /// <summary>
        /// Get the pinned list for the currently authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/userprofiles/me/lists/pinned")]
        [ProducesResponseType(typeof(ListResponse), 200)]
        public async Task<IActionResult> GetPinnedList()
        {
            var result = await _listReader.GetPinnedListByUserProfileId(_userProfileId);

            if (result == null)
            {
                return NoContent();
            }

            var authorisationResponse = await _listAuthoriser.IsOwner(result.Id, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var response = new ListResponse
            {
                Id = authorisationResponse.ResponseObject.Id,
                Name = authorisationResponse.ResponseObject.Name,
                Description = authorisationResponse.ResponseObject.Description,
                ImageSource = _cdnResolver.Resolve(authorisationResponse.ResponseObject.BackgroundImageFilePath),
                State = authorisationResponse.ResponseObject.State,
                Position = authorisationResponse.ResponseObject.Position
            };

            return Ok(response);
        }

        /// <summary>
        /// Pin a list for the currently authenticated user
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/userprofiles/me/lists/{listId}/pin")]
        public async Task<IActionResult> PinList(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            await _listWriter.PinList(_userProfileId, listId);

            return NoContent();
        }
    }
}