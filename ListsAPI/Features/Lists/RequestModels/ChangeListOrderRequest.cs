using System.Collections.Generic;

namespace ListsAPI.Features.Lists.RequestModels
{
    public class ChangeListOrderRequest
    {
        public List<int> ListIds { get; set; }
    }
}