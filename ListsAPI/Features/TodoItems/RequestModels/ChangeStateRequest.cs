using ListsAPI.Features.TodoItems.Enums;

namespace ListsAPI.Features.TodoItems.RequestModels
{
    public class ChangeStateRequest
    {
        public TodoItemState state { get; set; }
    }
}