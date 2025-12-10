using BlazorProj_CA3.Models;
using System.Threading.Tasks;

namespace BlazorProj_CA3.Services
{
    public interface IJokeService
    {
        Task<string> GetRandomJokeAsync(CancellationToken ct = default);
        Task<SearchResult> SearchJokesAsync(string term, int page = 1, int limit = 20, CancellationToken ct = default);
    }
}
