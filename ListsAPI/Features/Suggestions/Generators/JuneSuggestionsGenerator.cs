﻿using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class JuneSuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "June";

        public IEnumerable<SuggestionResponse> Generate()
        {
            var response = new List<SuggestionResponse>
            {
                BaseSuggestions.BlankList,
                BaseSuggestions.ShoppingList,
                BaseSuggestions.Tomorrow,
                BaseSuggestions.OddJobs
            };

            return response;
        }
    }
}