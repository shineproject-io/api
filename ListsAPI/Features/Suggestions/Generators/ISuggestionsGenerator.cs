using ListsAPI.Features.Suggestions.ResponseModels;
using System.Collections.Generic;

namespace ListsAPI.Features.Suggestions.Generators
{
    public interface ISuggestionsGenerator
    {
        string Month { get; }

        IEnumerable<SuggestionResponse> Generate();
    }
}