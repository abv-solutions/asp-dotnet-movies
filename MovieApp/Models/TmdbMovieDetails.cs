using System.Text.Json.Serialization;

namespace MovieApp.Models
{
    public class TmdbMovieDetails
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("overview")]
        public required string Overview { get; set; }

        [JsonPropertyName("vote_average")]
        public required float Score { get; set; }

        [JsonPropertyName("vote_count")]
        public required int ScoreCount { get; set; }

        [JsonPropertyName("budget")]
        public int Budget { get; set; }

        [JsonPropertyName("poster_path")]
        public required string PosterPath { get; set; }

        [JsonPropertyName("genres")]
        public required List<TmdbGenre> Genres { get; set; }
    }
}
