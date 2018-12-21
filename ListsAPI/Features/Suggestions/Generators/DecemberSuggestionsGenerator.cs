using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class DecemberSuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "December";

        public IEnumerable<SuggestionResponse> Generate()
        {
            var response = new List<SuggestionResponse>
            {
                BaseSuggestions.GetBlankList(),
                new SuggestionResponse
                {
                    SuggestionTitle = "Christmas is coming, are you ready?",
                    SuggestionDescription = "No need to stress, create a list to help with your Christmas shopping.",
                    SuggestionIcon = "fas fa-gift",
                    ListTitle = "Christmas shopping",
                    ListDescription = "Spread the Christmas cheer by remembering to get everybody a gift this year!",
                    ListBackgroundImageUrl = "https://shinestorage.azureedge.net/bespokebackgrounds/christmas.jpg"
                },
                BaseSuggestions.ShoppingList(),
                BaseSuggestions.Tomorrow(),
                BaseSuggestions.OddJobs()
            };

            return response;
        }
    }
}