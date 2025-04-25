using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _repository;

        public CommentsController(ICommentRepository repository)
        {
            _repository = repository;
        }

        // POST: /Comments
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create(int movieId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["Error"] = "Comment text cannot be empty.";
                return RedirectToAction("Details", "Movies", new { id = movieId });
            }

            var comment = new Comment
            {
                MovieId = movieId,
                Text = text,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddCommentAsync(comment);

            // Redirect back to the movie details page.
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        // GET: /Comments?movieId=123
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int movieId)
        {
            var comments = await _repository.GetCommentsByMovieIdAsync(movieId);
            return PartialView("_Comments", comments);
        }
    }
}
