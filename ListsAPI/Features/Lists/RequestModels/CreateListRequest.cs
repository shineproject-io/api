namespace ListsAPI.Features.Lists.RequestModels
{
    public class CreateListRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }
    }
}