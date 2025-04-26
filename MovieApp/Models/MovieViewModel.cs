namespace MovieApp.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Overview { get; set; }
        public double Score { get; set; }
        public int ScoreCount { get; set; }
        public int? Budget { get; set; }
        public string? PosterPath { get; set; }
        public List<TmdbGenre>? Genres { get; set; }
        public List<Comment> Comments { get; set; } = [];
        public List<TmdbCastMember> Cast { get; set; } = [];
        public List<TmdbImage> Images { get; set; } = [];
    }

}