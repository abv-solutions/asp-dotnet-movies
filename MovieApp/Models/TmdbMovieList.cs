using System.Text.Json.Serialization;

namespace MovieApp.Models
{
    public class TmdbMovieList
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public required List<TmdbMovie> Results { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }

    public class TmdbMovie
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
    }
}
    