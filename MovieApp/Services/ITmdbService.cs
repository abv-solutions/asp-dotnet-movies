using MovieApp.Models;

namespace MovieApp.Services
{
    public interface ITmdbService
    {
        Task<TmdbMovieList> GetLatestMoviesAsync();
        Task<TmdbMovieList> GetTopMoviesAsync();
        Task<TmdbMovieList> SearchMoviesAsync(string genreQuery, string query);
        Task<TmdbMovieDetails> GetMovieDetailsAsync(int movieId);
        Task<List<TmdbGenre>> GetGenresAsync();
        Task<TmdbMovieCredits> GetMovieCreditsAsync(int movieId);
        Task<TmdbMovieImages> GetMovieImagesAsync(int movieId);
    }
}
