using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.Search.Enums;
using ListsAPI.Features.Search.ResponseModels;
using ListsAPI.Features.TodoItems.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ListsAPI.Features.Search
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IListReader _listReader;
        private readonly ITodoItemsReader _taskReader;

        public SearchController(IListReader listReader, ITodoItemsReader taskReader)
        {
            _listReader = listReader;
            _taskReader = taskReader;
        }

        /// <summary>
        /// Search for a list or task
        /// </summary>
        /// <param name="searchQuery">The query to search</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/search")]
        [ProducesResponseType(typeof(IEnumerable<SearchResultResponse>), 200)]
        public async Task<IActionResult> Search(string searchQuery)
        {
            var userProfileId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var listResults = await _listReader.Search(searchQuery, userProfileId);

            var response = listResults.Select(res => new SearchResultResponse
            {
                ResultType = SearchResponseType.List.ToString(),
                ListId = res.Id,
                ResultDescription = res.Name
            }).ToList();

            var taskResults = await _taskReader.Search(searchQuery, userProfileId);

            if (taskResults.Any())
            {
                var taskResponse = taskResults.Select(tsk => new SearchResultResponse
                {
                    ResultType = SearchResponseType.Todo.ToString(),
                    ListId = tsk.ListId,
                    TodoId = tsk.Id,
                    ResultDescription = tsk.Title
                });

                response.AddRange(taskResponse);
            }

            for (int loop = 1; loop < response.Count; loop += 1)
            {
                response[loop].ResultId = loop;
            }

            return Ok(response);
        }
    }
}