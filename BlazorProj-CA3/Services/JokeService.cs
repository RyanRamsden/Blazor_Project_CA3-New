using BlazorProj_CA3.Models;
using BlazorProj_CA3.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorProj_CA3.Services
{
    public class JokeService : IJokeService
    {
        private readonly HttpClient _client;
        public JokeService(HttpClient client) => _client = client;

        public async Task<string> GetRandomJokeAsync(CancellationToken ct = default)
        {
            // GET / -> JSON with fields id, joke, status
            var resp = await _client.GetAsync("", ct);
            resp.EnsureSuccessStatusCode();
            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            if (doc.RootElement.TryGetProperty("joke", out var jokeElement))
                return jokeElement.GetString() ?? string.Empty;
            return string.Empty;
        }

        public async Task<SearchResult> SearchJokesAsync(string term, int page = 1, int limit = 20, CancellationToken ct = default)
        {
            var url = $"search?term={Uri.EscapeDataString(term)}&page={page}&limit={limit}";
            var resp = await _client.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"Search failed: {resp.StatusCode}");

            var result = await resp.Content.ReadFromJsonAsync<SearchResult>(cancellationToken: ct);
            return result ?? new SearchResult();
        }
    }
}
