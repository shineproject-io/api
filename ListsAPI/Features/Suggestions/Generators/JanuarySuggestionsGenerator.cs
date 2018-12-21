using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class JanuarySuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "January";

        public IEnumerable<SuggestionResponse> Generate()
        {
            var response = new List<SuggestionResponse>
            {
                BaseSuggestions.BlankList,
                new SuggestionResponse
                {
                    SuggestionTitle = "What are your new years resolutions?",
                    SuggestionDescription = "Keep track of your resolutions and make sure to see them through!",
                    SuggestionIcon = "fas fa-glass-cheers",
                    ListTitle = "New years resolutions",
                    ListDescription = "These are the things I want to do better this year.",
                    ListBackgroundImageUrl = "https://shinestorage.azureedge.net/productlistbackgrounds/1.jpg"
                },
                BaseSuggestions.ShoppingList,
                BaseSuggestions.Tomorrow,
                BaseSuggestions.OddJobs
            };

            return response;
        }
    }
}