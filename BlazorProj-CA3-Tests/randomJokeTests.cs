using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlazorProj_CA3_Tests
{
        [Collection("e2e")]
    public class RandomJokeTests
    {
        private readonly BlazorProjTests _bp;
        public RandomJokeTests(BlazorProjTests bp) => _bp = bp;

        [Fact]
        public async Task RandomJoke_Displays_Text_After_Fetch()
        {
            await _bp.Page.GotoAsync($"{_bp.BaseUrl}/random", new() { WaitUntil = Microsoft.Playwright.WaitUntilState.NetworkIdle });

            var fetchBtn = _bp.Page.GetByTestId("fetch-random-btn");
            await fetchBtn.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await fetchBtn.ClickAsync();


            // Wait for joke text to appear and have content
            var jokeText = _bp.Page.GetByTestId("random-joke-text");
            await jokeText.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            // Assert it's not empty and looks like a sentence
            var text = await jokeText.InnerTextAsync();
            Assert.False(string.IsNullOrWhiteSpace(text));
            Assert.True(text.Length > 10);
        }
    }
}
