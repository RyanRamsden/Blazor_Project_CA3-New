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
            await _bp.Page.GotoAsync($"{_bp.BaseUrl}/random", new() { WaitUntil = WaitUntilState.DOMContentLoaded });

            // Wait for Blazor root element to render
            await _bp.Page.WaitForSelectorAsync("div#app", new PageWaitForSelectorOptions { Timeout = 60000 });

            var fetchBtn = _bp.Page.GetByTestId("fetch-random-btn");
            await fetchBtn.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });
            await fetchBtn.ClickAsync();

            var jokeText = _bp.Page.GetByTestId("random-joke-text");
            await jokeText.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });

            var text = await jokeText.InnerTextAsync();
            Assert.False(string.IsNullOrWhiteSpace(text));
            Assert.True(text.Length > 10);

            // Debug dump
            var content = await _bp.Page.ContentAsync();
            Console.WriteLine("Page content:\n" + content);
        }
    }
}
