using ListsAPI.Features.Lists.Enums;

namespace ListsAPI.Features.Lists.RequestModels
{
    public class ChangeStateRequest
    {
        public ListState State { get; set; }
    }
}