using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace BlazorProj_CA3_Tests
{
    public class BlazorProjTests : IAsyncLifetime
    {
        public IPlaywright Playwright { get; private set; } = default!;
        public IBrowser Browser { get; private set; } = default!;
        public IBrowserContext Context { get; private set; } = default!;
        public IPage Page { get; private set; } = default!;
        public string BaseUrl { get; private set; } = Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "https://localhost:7004";
        private Process? _appProcess;

        public async Task InitializeAsync()
        {
            // Start the Blazor app before running tests
            _appProcess = StartApp(@"..\..\..\BlazorProj-CA3\BlazorProj-CA3.csproj");
            await WaitForApp($"{BaseUrl}/search");


            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {Headless = true});
            Context = await Browser.NewContextAsync(new BrowserNewContextOptions {IgnoreHTTPSErrors = true});
            Page = await Context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await Page.CloseAsync();
            await Context.CloseAsync();
            await Browser.CloseAsync();
            Playwright.Dispose();

            if (_appProcess is { HasExited: false })
            {
                _appProcess.Kill(true);
                _appProcess.Dispose();
            }
        }

        private Process StartApp(string projectPath)
        {
            var psi = new ProcessStartInfo("dotnet", $"run --project \"{projectPath}\"")
            {
                WorkingDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..")),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var proc = Process.Start(psi)!;
            return proc;
        }

        private async Task WaitForApp(string url, int timeoutMs = 120000)
        {
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            using var client = new HttpClient();

            while (DateTime.UtcNow < deadline)
            {
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return; //app is up
                    }
                }
                catch
                {
                    //ignore and retry
                }

                await Task.Delay(1000);
            }

            throw new TimeoutException($"App did not start at {url} in {timeoutMs}ms.");
        }


    }

    [CollectionDefinition("e2e")]
    public class E2ECollection : ICollectionFixture<BlazorProjTests> { }
}
