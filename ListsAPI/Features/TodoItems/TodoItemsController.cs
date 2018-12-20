using ListsAPI.Features.Lists.Authorizers;
using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.TodoItems.DataAccess;
using ListsAPI.Features.TodoItems.Enums;
using ListsAPI.Features.TodoItems.RequestModels;
using ListsAPI.Features.TodoItems.ResponseModels;
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
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsWriter _todoItemsWriter;
        private readonly ITodoItemsReader _todoItemsReader;
        private readonly IListAuthoriser _listAuthoriser;

        private int _userProfileId => Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

        public TodoItemsController(ITodoItemsWriter todoItemsWriter, ITodoItemsReader todoItemsReader, IListReader listReader, IListAuthoriser listAuthoriser)
        {
            _todoItemsWriter = todoItemsWriter;
            _todoItemsReader = todoItemsReader;
            _listAuthoriser = listAuthoriser;
        }

        /// <summary>
        /// Get all Todo items in a list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/lists/{listId}/todoItems")]
        [ProducesResponseType(typeof(IEnumerable<TodoItemResponse>), 200)]
        public async Task<IActionResult> Get(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var todoItems = await _todoItemsReader.GetByListId(listId);

            var response = todoItems
                .Select(tsk => new TodoItemResponse
                {
                    Id = tsk.Id,
                    Title = tsk.Title,
                    State = tsk.State.ToString(),
                    Position = tsk.Position
                });

            return Ok(response);
        }

        /// <summary>
        /// Create a new Todo Item
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lists/{listId}/todoItems")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> Post(int listId, CreateTodoItemRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var todoId = await _todoItemsWriter.Add(request.Title, TodoItemState.Open, listId, _userProfileId);

            return Ok(todoId);
        }

        /// <summary>
        /// Change the state of a todo item
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="todoItemId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/lists/{listId}/todoItems/{todoItemId}/state")]
        public async Task<IActionResult> ChangeState(int listId, int todoItemId, ChangeTodoItemStateRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var todoItem = await _todoItemsReader.GetByTodoItemId(todoItemId);

            if (todoItem == null || todoItem.UserProfileId != _userProfileId)
            {
                return NotFound();
            }

            await _todoItemsWriter.ChangeState(todoItem.Id, request.state);

            return Ok();
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="todoItemId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/lists/{listId}/todoItems/{todoItemId}")]
        public async Task<IActionResult> Delete(int listId, int todoItemId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var todoItem = await _todoItemsReader.GetByTodoItemId(todoItemId);

            if (todoItem == null || todoItem.UserProfileId != _userProfileId)
            {
                return NotFound();
            }

            await _todoItemsWriter.ChangeState(todoItem.Id, TodoItemState.Deleted);

            return Ok();
        }

        /// <summary>
        /// Delete all Todo Items in the list
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/lists/{listId}/todoItems")]
        public async Task<IActionResult> DeleteAllTodoItems(int listId)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            await _todoItemsWriter.DeleteAllTodoItemsByListId(listId);

            return Ok();
        }

        /// <summary>
        /// Change the title of a Todo Item
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="todoItemId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/lists/{listId}/todoItems/{todoItemId}/title")]
        public async Task<IActionResult> ChangeTodoItemTitle(int listId, int todoItemId, EditTitleRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            var todoItem = await _todoItemsReader.GetByTodoItemId(todoItemId);

            if (todoItem == null || todoItem.UserProfileId != _userProfileId)
            {
                return NotFound();
            }

            await _todoItemsWriter.ChangeTitle(todoItem.Id, request.Title);

            return Ok();
        }

        /// <summary>
        /// Change the order that tasks are displayed in the specified list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lists/{listId}/todoItems/order")]
        public async Task<IActionResult> ChangeOrder(int listId, ChangeOrderRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            await _todoItemsWriter.ChangeOrder(listId, request.todoItemIds);

            return Ok();
        }

        /// <summary>
        /// Create all welcome Todo Items in the specified list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lists/{listId}/welcome")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateWelcomeList(int listId)
        {
            await _todoItemsWriter.Add("You can change the list name by selecting it and changing the text", TodoItemState.Open, listId, _userProfileId);
            await _todoItemsWriter.Add("Complete a to-do by selecting the circle next to it", TodoItemState.Open, listId, _userProfileId);
            await _todoItemsWriter.Add("Drag your to-do's into any order you want with the handle", TodoItemState.Open, listId, _userProfileId);
            await _todoItemsWriter.Add("Create a new list by selecting the (+) button in the corner", TodoItemState.Open, listId, _userProfileId);
            await _todoItemsWriter.Add("Add a new to-do by selecting 'What do you want to do next?'", TodoItemState.Open, listId, _userProfileId);
            await _todoItemsWriter.Add("Now lets mark everything as completed so we can finish the list", TodoItemState.Open, listId, _userProfileId);

            return Ok();
        }

        /// <summary>
        /// Move any open tasks from one list to another
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lists/{listId}/migrate")]
        public async Task<IActionResult> MigrateTodoItems(int listId, MigrateListRequest request)
        {
            var authorisationResponse = await _listAuthoriser.IsOwner(listId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            authorisationResponse = await _listAuthoriser.IsOwner(request.NewListId, _userProfileId);

            if (!authorisationResponse.AuthorisationResult)
            {
                return NotFound();
            }

            if (authorisationResponse.ResponseObject.State != Enums.ListState.Open)
            {
                return BadRequest("Selected list is not active");
            }

            await _todoItemsWriter.MigrateOpenTodoItemsToListById(listId, request.NewListId);

            return NoContent();
        }
    }
}