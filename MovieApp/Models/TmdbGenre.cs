using System.Text.Json.Serialization;

namespace MovieApp.Models
{
    public class TmdbGenre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; } = string.Empty;
    }
}
