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
    public class SearchJokeTests
    {
        private readonly BlazorProjTests _bp;
        public SearchJokeTests(BlazorProjTests bp) => _bp = bp;

        [Fact]
        public async Task Search_Shows_Results_For_Common_Query()
        {
            await _bp.Page.GotoAsync($"{_bp.BaseUrl}/search", new() { WaitUntil = WaitUntilState.DOMContentLoaded });

            // Wait for Blazor root element to render
            await _bp.Page.WaitForSelectorAsync("div#app", new PageWaitForSelectorOptions { Timeout = 60000 });

            var input = _bp.Page.GetByTestId("keyword-search");
            await input.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });
            await input.FillAsync("dog");

            var button = _bp.Page.GetByTestId("search-btn");
            await button.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });
            await button.ClickAsync();

            var list = _bp.Page.GetByTestId("results-list");
            await list.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });

            var items = _bp.Page.GetByTestId("results-item");
            await _bp.Page.WaitForFunctionAsync(
                @"() => document.querySelectorAll('[data-testid=""results-item""]').length > 0",
                new PageWaitForFunctionOptions { Timeout = 60000 }
            );

            var count = await items.CountAsync();
            Assert.True(count > 0, "Expected at least one joke result for 'dog'.");

            // Debug dump
            var content = await _bp.Page.ContentAsync();
            Console.WriteLine("Page content:\n" + content);
        }
    }
}