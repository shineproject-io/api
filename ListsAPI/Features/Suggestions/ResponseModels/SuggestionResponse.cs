namespace ListsAPI.Features.Suggestions.ResponseModels
{
    public class SuggestionResponse
    {
        public string SuggestionTitle { get; set; }
        public string SuggestionDescription { get; set; }
        public string SuggestionIcon { get; set; }

        public string ListTitle { get; set; }
        public string ListDescription { get; set; }
        public string ListBackgroundImageUrl { get; set; }
    }
}