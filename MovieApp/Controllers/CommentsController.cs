using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    public class CommentsController(ICommentRepository repository) : Controller
    {
        private readonly ICommentRepository _repository = repository;

        // POST: /Comments
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int movieId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["Error"] = "Comment text cannot be empty.";
                return RedirectToAction("Details", "Movies", new { id = movieId });
            }

            if (movieId <= 0)
            {
                TempData["Error"] = "Invalid movie.";
                return RedirectToAction("Index", "Movies");
            }

            var comment = new Comment
            {
                MovieId = movieId,
                Text = text,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddCommentAsync(comment);

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        // GET: /Comments?movieId=123
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int movieId)
        {
            if (movieId <= 0)
            {
                return BadRequest("Invalid movie ID.");
            }

            var comments = await _repository.GetCommentsByMovieIdAsync(movieId);
            return PartialView("_Comments", comments);
        }
    }
}
