using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class NovemberSuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "November";

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