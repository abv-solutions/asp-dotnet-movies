namespace MovieApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public required string UserId { get; set; }

        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
