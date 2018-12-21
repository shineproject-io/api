using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class SeptemberSuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "September";

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