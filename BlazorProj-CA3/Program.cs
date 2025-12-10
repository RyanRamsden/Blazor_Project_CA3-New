using BlazorProj_CA3;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;
using BlazorProj_CA3.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Move the hardcoded URI to a configuration value
var dadJokeApiBaseUrl = builder.Configuration["DadJokeApi:BaseUrl"] ?? "https://icanhazdadjoke.com/";

builder.Services.AddHttpClient<IJokeService, JokeService>(client =>
{
    client.BaseAddress = new Uri(dadJokeApiBaseUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

await builder.Build().RunAsync();
