namespace ListsAPI.Features.Search.ResponseModels
{
    public class SearchResultResponse
    {
        public int ResultId { get; set; }

        public string ResultType { get; set; }

        public string ResultDescription { get; set; }

        public int? ListId { get; set; }

        public int? TodoId { get; set; }
    }
}