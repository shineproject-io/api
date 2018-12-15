using System.Collections.Generic;

namespace ListsAPI.Features.TodoItems.ResponseModels
{
    public class ChangeOrderRequest
    {
        public List<int> todoItemIds { get; set; }
    }
}