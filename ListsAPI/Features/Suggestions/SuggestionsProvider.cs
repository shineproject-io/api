using ListsAPI.Features.Suggestions.Generators;
using ListsAPI.Features.Suggestions.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListsAPI.Features.Suggestions.Providers
{
    public interface ISuggestionsProvider
    {
        IEnumerable<SuggestionResponse> Provide(DateTime currentDateTime);
    }

    public class SuggestionsProvider : ISuggestionsProvider
    {
        private readonly IEnumerable<ISuggestionsGenerator> _suggestionsGenerators;

        public SuggestionsProvider(IEnumerable<ISuggestionsGenerator> suggestionsGenerators)
        {
            _suggestionsGenerators = suggestionsGenerators;
        }

        public IEnumerable<SuggestionResponse> Provide(DateTime currentDateTime)
        {
            var currentMonth = currentDateTime.ToString("MMMM");

            var selectedGenerator = _suggestionsGenerators.FirstOrDefault(x => currentMonth.Equals(x.Month, StringComparison.OrdinalIgnoreCase));

            var suggestions = selectedGenerator.Generate();

            return suggestions;
        }
    }
}