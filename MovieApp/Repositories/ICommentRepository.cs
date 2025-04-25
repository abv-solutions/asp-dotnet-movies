using MovieApp.Models;

namespace MovieApp.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByMovieIdAsync(int movieId);
    }
}
