namespace BlazorProj_CA3.Models
{
    public class SearchResult
    {
        public int Current_Page { get; set; }
        public int Limit { get; set; }
        public int Next_Page { get; set; }
        public int Previous_Page { get; set; }
        public int Total_Jokes { get; set; }
        public int Total_Pages { get; set; }
        public List<SearchJokeItem> Results { get; set; } = new();
    }

    public class SearchJokeItem
    {
        public string Id { get; set; }
        public string Joke { get; set; }
    }
}
