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

    public class TmdbCastMember
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("character")]
        public required string Character { get; set; }

        [JsonPropertyName("profile_path")]
        public required string ProfilePath { get; set; }

    }

    public class TmdbMovieCredits
    {
        [JsonPropertyName("cast")]
        public required List<TmdbCastMember> Cast { get; set; }
    }

    public class TmdbMovieImages
    {
        [JsonPropertyName("backdrops")]
        public required List<TmdbImage> Backdrops { get; set; }

        [JsonPropertyName("posters")]
        public required List<TmdbImage> Posters { get; set; }
    }

    public class TmdbImage
    {
        [JsonPropertyName("file_path")]
        public required string FilePath { get; set; }
    }

}
