using ListsAPI.Features.TodoItems.Enums;

namespace ListsAPI.Features.TodoItems.RequestModels
{
    public class ChangeTodoItemStateRequest
    {
        public TodoItemState state { get; set; }
    }
}