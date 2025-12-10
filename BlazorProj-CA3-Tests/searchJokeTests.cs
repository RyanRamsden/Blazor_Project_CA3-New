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
            await _bp.Page.GotoAsync($"{_bp.BaseUrl}/search", new() { WaitUntil = WaitUntilState.NetworkIdle });
            await _bp.Page.WaitForSelectorAsync("body");


            var input = _bp.Page.GetByTestId("keyword-search");
            await input.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await input.FillAsync("dog");

            var button = _bp.Page.GetByTestId("search-btn");
            await button.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await button.ClickAsync();


            // Wait for results list to become visible or populated
            var list = _bp.Page.GetByTestId("results-list");
            await list.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var items = _bp.Page.GetByTestId("results-item");
            // Wait until at least one item exists
            await _bp.Page.WaitForFunctionAsync(@"(el) => document.querySelectorAll('[data-testid=""results-item""]').length > 0");

            var count = await items.CountAsync();
            Assert.True(count > 0, "Expected at least one joke result for 'dog'.");
        }
    }
}