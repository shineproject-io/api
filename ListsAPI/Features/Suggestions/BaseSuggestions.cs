using ListsAPI.Features.Suggestions.ResponseModels;
using System;

namespace ListsAPI.Features.Suggestions.Generators
{
    public static class BaseSuggestions
    {
        public static SuggestionResponse BlankList = new SuggestionResponse
        {
            SuggestionTitle = "Start an empty list",
            SuggestionDescription = "How about a fresh canvas to start jotting down your thoughts?",
            SuggestionIcon = "fas fa-plus",
            ListTitle = "Blank list",
            ListDescription = "What are you hoping to achieve?",
            ListBackgroundImageUrl = "https://shinestorage.azureedge.net/productlistbackgrounds/1.jpg"
        };

        public static SuggestionResponse ShoppingList = new SuggestionResponse
        {
            SuggestionTitle = "Planning to hit the shops?",
            SuggestionDescription = "Start a new shopping list so you remember to get everything you need!",
            SuggestionIcon = "fas fa-shopping-basket",
            ListTitle = "Shopping list",
            ListDescription = "Remember to buy these things when you get to the shops",
            ListBackgroundImageUrl = "https://shinestorage.azureedge.net/bespokebackgrounds/shopping.jpg"
        };

        public static SuggestionResponse OddJobs = new SuggestionResponse
        {
            SuggestionTitle = "Sorting out some odd jobs?",
            SuggestionDescription = "Nobody likes chores, but the sooner you get them done, the better.",
            SuggestionIcon = "fas fa-bolt",
            ListTitle = "Odd jobs",
            ListDescription = "These things have been building up, its time to smash them",
            ListBackgroundImageUrl = "https://shinestorage.azureedge.net/bespokebackgrounds/jobs.jpg"
        };

        public static SuggestionResponse Tomorrow = new SuggestionResponse
        {
            SuggestionTitle = "Need to organise your day tomorrow?",
            SuggestionDescription = "We can create a list to help you suceed tomorrow. Plan tomorrow, today.",
            SuggestionIcon = "fas fa-calendar",
            ListTitle = DateTime.UtcNow.AddDays(1).ToString("dd MMMM yyyy"),
            ListDescription = "Failing to plan is planning to fail, so today we are going to do things the right way",
            ListBackgroundImageUrl = "https://shinestorage.azureedge.net/bespokebackgrounds/tomorrow.jpg"
        };
    }
}