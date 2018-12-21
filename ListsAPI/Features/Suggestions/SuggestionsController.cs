using ListsAPI.Features.Suggestions.Providers;
using ListsAPI.Features.Suggestions.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions
{
    [Authorize]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionsProvider _suggestionsProvider;

        public SuggestionsController(ISuggestionsProvider suggestionsResolver)
        {
            _suggestionsProvider = suggestionsResolver;
        }

        /// <summary>
        /// Get a list of suggestions for a new list based on the current date
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/lists/suggestions")]
        [ProducesResponseType(typeof(IEnumerable<SuggestionResponse>), 200)]
        public IActionResult Index()
        {
            var response = _suggestionsProvider.Provide(DateTime.UtcNow);

            return Ok(response);
        }
    }
}