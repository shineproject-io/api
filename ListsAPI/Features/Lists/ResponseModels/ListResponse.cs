using ListsAPI.Features.Lists.Enums;

namespace ListsAPI.Features.Lists.ResponseModels
{
    public class ListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        public ListState State { get; set; }

        public int? Position { get; set; }
    }
}