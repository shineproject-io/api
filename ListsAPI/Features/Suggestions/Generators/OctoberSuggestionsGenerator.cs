using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class OctoberSuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "October";

        public IEnumerable<SuggestionResponse> Generate()
        {
            var response = new List<SuggestionResponse>
            {
                BaseSuggestions.GetBlankList(),
                BaseSuggestions.ShoppingList(),
                BaseSuggestions.Tomorrow(),
                BaseSuggestions.OddJobs()
            };

            return response;
        }
    }
}