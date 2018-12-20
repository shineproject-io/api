using ListsAPI.Features.Lists.Authorizers;
using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.Lists.Enums;
using ListsAPI.Features.Lists.RequestModels;
using ListsAPI.Features.Lists.ResponseModels;
using ListsAPI.Infrastructure.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists
{
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListAuthoriser _listAuthoriser;
        private readonly IListWriter _listWriter;
        private readonly IListReader _listReader;
        private readonly IAzureStorageManager _storeFile;

        private int _userProfileId => Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

        private const string CONTAINER_NAME = "listbackgrounds";

        public ListsController(IListAuthoriser listAuthoriser, IListWriter listWriter, IListReader listReader, IAzureStorageManager storeFile)
        {
            _listAuthoriser = listAuthoriser;
            _listWriter = listWriter;
            _listReader = listReader;
            _storeFile = storeFile;
        }

        /// <summary>
        /// Get all open lists
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/lists")]
        [ProducesResponseType(typeof(IEnumerable<ListResponse>), 200)]
        public async Task<IActionResult> Get()
        {
            var lists = await _listReader.GetByState(_userProfileId, ListState.Open);

            var response = lists
                   .Select(lst => new ListResponse
                   {
                       Id = lst.Id,
                       Name = lst.Name,
                       Description = lst.Description,
                       ImageSource = lst.BackgroundImageFilePath,
                       State = lst.State,
                       Position = lst.Position
                   });

            return Ok(response);
        }

        /// <summary>
        /// Get a specific list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/lists/{listId}")]
        [ProducesResponseType(typeof(ListResponse), 200)]
        public async Task<IActionResult> GetByListId(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var response = new ListResponse
            {
                Id = authorisationResponse.ResponseObject.Id,
                Name = authorisationResponse.ResponseObject.Name,
                Description = authorisationResponse.ResponseObject.Description,
                ImageSource = authorisationResponse.ResponseObject.BackgroundImageFilePath,
                State = authorisationResponse.ResponseObject.State,
                Position = authorisationResponse.ResponseObject.Position
            };

            return Ok(response);
        }

        /// <summary>
        /// Create a new list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/lists")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> Post(CreateListRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageSource))
            {
                return BadRequest("An image source must be specified");
            }

            var listId = await _listWriter.Add(_userProfileId, request.Name, request.Description, ListState.Open, request.ImageSource);

            return Ok(listId);
        }

        /// <summary>
        /// Delete a list
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/lists/{listId}")]
        public async Task<IActionResult> Delete(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var existingBackgroundPictureFileName = authorisationResponse.ResponseObject.BackgroundImageFileName;

            if (!string.IsNullOrEmpty(existingBackgroundPictureFileName))
            {
                await _storeFile.DeleteFile(CONTAINER_NAME, existingBackgroundPictureFileName);
            }

            await _listWriter.ChangeState(authorisationResponse.ResponseObject.Id, ListState.Deleted);

            return Ok();
        }

        /// <summary>
        /// Update the name of a list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/lists/{listId}/name")]
        public async Task<IActionResult> ChangeName(int listId, ChangeNameRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            await _listWriter.ChangeName(listId, request.Name);

            return Ok();
        }

        /// <summary>
        /// Update the description of a list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/lists/{listId}/description")]
        public async Task<IActionResult> ChangeDescription(int listId, ChangeDescriptionRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            await _listWriter.ChangeDescription(listId, request.Description);

            return Ok();
        }

        /// <summary>
        /// Change the display order of lists
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/lists/order")]
        public async Task<IActionResult> ChangeOrder(ChangeListOrderRequest request)
        {
            var lists = await _listReader.GetByState(_userProfileId, ListState.Open);

            _listWriter.ChangeOrder(lists, request.ListIds);

            return Ok();
        }

        /// <summary>
        /// Change the state of a list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/lists/{listId}/state")]
        public async Task<IActionResult> ChangeState(int listId, ChangeStateRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            if (request.State == ListState.Deleted)
            {
                return BadRequest("Unable to delete a list through this endpoint");
            }

            await _listWriter.ChangeState(listId, request.State);

            return Ok();
        }

        /// <summary>
        /// Change the image in the background of a list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/lists/{listId}/picture")]
        public async Task<IActionResult> ChangePicture(int listId, ChangePictureRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var existingBackgroundPictureFileName = authorisationResponse.ResponseObject.BackgroundImageFileName;

            await _listWriter.ChangePicture(listId, request.ImageSource, string.Empty);

            if (!string.IsNullOrEmpty(existingBackgroundPictureFileName))
            {
                await _storeFile.DeleteFile(CONTAINER_NAME, existingBackgroundPictureFileName);
            }

            return Ok();
        }

        /// <summary>
        /// Upload a new picture for the list background
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/lists/{listId}/picture/upload")]
        public async Task<IActionResult> UploadPicture(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            if (Request.Form.Files.Count == 0)
            {
                return BadRequest("You must attach an image");
            }

            var listBackgroundPhoto = Request.Form.Files[0];

            if (listBackgroundPhoto.Length > 6000000)
            {
                return StatusCode(413);
            }

            var acceptedContentTypes = new List<string> { "image/png", "image/jpg", "image/jpeg", "image/gif" };

            if (!acceptedContentTypes.Contains(listBackgroundPhoto.ContentType))
            {
                return StatusCode(415);
            }

            var fileName = Guid.NewGuid().ToString();

            var existingBackgroundPictureFileName = authorisationResponse.ResponseObject.BackgroundImageFileName;

            var absoluteFilePath = await _storeFile.StoreFile(CONTAINER_NAME, fileName, listBackgroundPhoto);

            await _listWriter.ChangePicture(listId, absoluteFilePath, fileName);

            if (!string.IsNullOrEmpty(existingBackgroundPictureFileName))
            {
                await _storeFile.DeleteFile(CONTAINER_NAME, existingBackgroundPictureFileName);
            }

            return NoContent();
        }

        /// <summary>
        /// Create a welcome list for the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/lists/welcome")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateWelcomeList()
        {
            var listId = await _listWriter.Add(
                _userProfileId,
                "Welcome to Shine",
                "It's great to have you on board, check out the to-dos below to get started.",
                ListState.Open,
                "https://listsstorage.blob.core.windows.net/productlistbackgrounds/6.jpg"
            );

            return Ok(listId);
        }
    }
}