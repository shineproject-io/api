namespace ListsAPI.Features.TodoItems.ResponseModels
{
    public class TodoItemResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public bool IsImportant { get; set; }

        public int? Position { get; set; }
    }
}