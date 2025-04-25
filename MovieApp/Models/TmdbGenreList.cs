using System.Text.Json.Serialization;

namespace MovieApp.Models
{
    public class TmdbGenreList
    {
        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; set; } = [];
    }
}
