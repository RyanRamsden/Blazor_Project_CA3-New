namespace BlazorProj_CA3.Models
{
    public class Joke
    {
        public string Id { get; set; } = string.Empty;
        public string JokeText { get; set; } = string.Empty; // map from JSON "joke"
        public int Status { get; set; }
    }
}
