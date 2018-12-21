using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public class FebruarySuggestionsGenerator : ISuggestionsGenerator
    {
        public string Month => "February";

        public IEnumerable<SuggestionResponse> Generate()
        {
            var response = new List<SuggestionResponse>
            {
                BaseSuggestions.GetBlankList(),
                new SuggestionResponse
                {
                    SuggestionTitle = "Fancy jetting off this summer?",
                    SuggestionDescription = "If you want to jet away this summer, its time to start planning.",
                    SuggestionIcon = "fas fa-place",
                    ListTitle = "Summer holiday planning",
                    ListDescription = "These are the things we need to do to make our holiday a reality",
                    ListBackgroundImageUrl = "https://shinestorage.azureedge.net/productlistbackgrounds/3.jpg"
                },
                BaseSuggestions.ShoppingList(),
                new SuggestionResponse
                {
                    SuggestionTitle = "Are you ready for valentines day?",
                    SuggestionDescription = "Maybe you want a space to think about what to do this year?",
                    SuggestionIcon = "fas fa-heart",
                    ListTitle = "Valentines day",
                    ListDescription = "Lets plan something special for valentines day this year",
                    ListBackgroundImageUrl = "https://shinestorage.azureedge.net/productlistbackgrounds/5.jpg"
                },
                BaseSuggestions.Tomorrow(),
            };

            return response;
        }
    }
}