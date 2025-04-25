using Microsoft.AspNetCore.Mvc;
using MovieApp.Services;
using MovieApp.Models;
using MovieApp.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ITmdbService _tmdbService;
        private readonly ICommentRepository _commentRepository;

        public MoviesController(ITmdbService tmdbService, ICommentRepository commentRepository)
        {
            _tmdbService = tmdbService;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index(string? query, List<int>? selectedGenres)
        {
            TmdbMovieList movies;
            var genres = await _tmdbService.GetGenresAsync();
            ViewData["Genres"] = genres;

            if (string.IsNullOrWhiteSpace(query) && (selectedGenres == null || selectedGenres.Count == 0))
            {
                movies = await _tmdbService.GetTopMoviesAsync();
                ViewData["Title"] = "Top Rated Movies";
            }
            else
            {
                var genreQuery = string.Join(",", selectedGenres);
                movies = await _tmdbService.SearchMoviesAsync(genreQuery, query);
                ViewData["Title"] = "Search Results";
            }
            return MapMovieList(movies);
        }

        // Action to fetch and return now playing movies
        public async Task<IActionResult> Latest()
        {
            var movies = await _tmdbService.GetLatestMoviesAsync();
            return MapMovieList(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _tmdbService.GetMovieDetailsAsync(id);
            var comments = await _commentRepository.GetCommentsByMovieIdAsync(id);
            return MapMovieDetails(movieDetails, comments);
        }

        public async Task<IActionResult> Genres()
        {
            var genres = await _tmdbService.GetGenresAsync();
            return View(genres);
        }

        public IActionResult Search(string query, List<int> selectedGenres)
        {
            return RedirectToAction("Index", new { query, selectedGenres });
        }

        private ViewResult MapMovieList(TmdbMovieList movies)
        {
            // Convert TmdbMovieList to List<MovieViewModel>
            var movieListViewModel = movies.Results.Select(movie => new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Score = movie.Score,
                ScoreCount = movie.ScoreCount
            }).ToList();

            return View(movieListViewModel);
        }

        private ViewResult MapMovieDetails(TmdbMovieDetails movie, List<Comment> comments)
        {
            // Convert TmdbMovie to MovieViewModel
            var movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Score = movie.Score,
                ScoreCount = movie.ScoreCount,
                Budget = movie.Budget,
                PosterPath = movie.PosterPath,
                Genres = movie.Genres,
                Comments = comments
            };

            return View(movieViewModel);
        }
    }
}
