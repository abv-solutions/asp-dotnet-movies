using Microsoft.AspNetCore.Mvc;
using MovieApp.Services;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    public class MoviesController(ITmdbService tmdbService, ICommentRepository commentRepository) : Controller
    {
        private readonly ITmdbService _tmdbService = tmdbService;
        private readonly ICommentRepository _commentRepository = commentRepository;

        [HttpGet]
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
                var genreQuery = BuildGenreQuery(selectedGenres);
                movies = await _tmdbService.SearchMoviesAsync(genreQuery, query);
                ViewData["Title"] = "Search Results";
            }

            return MapMovieList(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Latest()
        {
            var movies = await _tmdbService.GetLatestMoviesAsync();
            return MapMovieList(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _tmdbService.GetMovieDetailsAsync(id);
            if (movieDetails == null)
            {
                return NotFound();
            }

            var comments = await _commentRepository.GetCommentsByMovieIdAsync(id);
            var credits = await _tmdbService.GetMovieCreditsAsync(id);
            var images = await _tmdbService.GetMovieImagesAsync(id);

            return MapMovieDetails(movieDetails, comments, credits, images);
        }

        [HttpGet]
        public async Task<IActionResult> Genres()
        {
            var genres = await _tmdbService.GetGenresAsync();
            return View(genres);
        }

        [HttpGet]
        public IActionResult Search(string? query, List<int>? selectedGenres)
        {
            return RedirectToAction(nameof(Index), new { query, selectedGenres });
        }

        private ViewResult MapMovieList(TmdbMovieList movies)
        {
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

        private ViewResult MapMovieDetails(TmdbMovieDetails movie, List<Comment> comments, TmdbMovieCredits credits, TmdbMovieImages images)
        {
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
                Comments = comments,
                Cast = credits.Cast,
                Images = images.Posters,
            };

            return View(movieViewModel);
        }

        private static string BuildGenreQuery(List<int>? selectedGenres)
        {
            return selectedGenres == null ? string.Empty : string.Join(",", selectedGenres);
        }
    }
}
