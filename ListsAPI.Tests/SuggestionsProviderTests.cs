using ListsAPI.Features.Suggestions.Generators;
using ListsAPI.Features.Suggestions.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ListsApi.Tests
{
    [TestFixture]
    public class SuggestionsProviderTests
    {
        [Test]
        public void Verify_ThatEachMonth_HasAValidGenerator()
        {
            var suggestionGenerators = new List<ISuggestionsGenerator>
            {
                new JanuarySuggestionsGenerator(),
                new FebruarySuggestionsGenerator(),
                new MarchSuggestionsGenerator(),
                new AprilSuggestionsGenerator(),
                new MaySuggestionsGenerator(),
                new JuneSuggestionsGenerator(),
                new JulySuggestionsGenerator(),
                new AugustSuggestionsGenerator(),
                new SeptemberSuggestionsGenerator(),
                new OctoberSuggestionsGenerator(),
                new NovemberSuggestionsGenerator(),
                new DecemberSuggestionsGenerator()
            };

            ISuggestionsProvider suggestionsProvider = new SuggestionsProvider(suggestionGenerators);

            int validMonths = 0;

            for (var month = 1; month <= 12; month += 1)
            {
                try
                {
                    var suggestions = suggestionsProvider.Provide(new DateTime(2018, month, 1));

                    validMonths++;
                }
                catch (Exception)
                {
                    Assert.Fail($"No generator found for month: {month}");
                }
            }

            Assert.AreEqual(12, validMonths);
            Assert.AreEqual(12, suggestionGenerators.Count);
        }
    }
}