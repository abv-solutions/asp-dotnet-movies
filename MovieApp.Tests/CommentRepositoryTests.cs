using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Tests
{
    public class CommentRepositoryTests
    {
        // Helper method to create an in-memory ApplicationDbContext
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddCommentAsync_Should_AddComment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CommentRepository(context);
            var comment = new Comment
            {
                MovieId = 123,
                Text = "Great movie!",
                UserId = "TestUser",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var addedComment = await repository.AddCommentAsync(comment);

            // Assert
            Assert.NotNull(addedComment);
            Assert.NotEqual(0, addedComment.Id);
            var retrieved = await context.Comments.FindAsync(addedComment.Id);
            Assert.Equal("Great movie!", retrieved.Text);
        }

        [Fact]
        public async Task GetCommentsByMovieIdAsync_Should_Return_CorrectComments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CommentRepository(context);
            var comment1 = new Comment { MovieId = 123, Text = "Comment 1", UserId = "TestUser", CreatedAt = DateTime.UtcNow };
            var comment2 = new Comment { MovieId = 123, Text = "Comment 2", UserId = "TestUser", CreatedAt = DateTime.UtcNow };
            var comment3 = new Comment { MovieId = 456, Text = "Comment 3", UserId = "TestUser", CreatedAt = DateTime.UtcNow };

            await repository.AddCommentAsync(comment1);
            await repository.AddCommentAsync(comment2);
            await repository.AddCommentAsync(comment3);

            // Act
            var comments = await repository.GetCommentsByMovieIdAsync(123);

            // Assert
            Assert.Equal(2, comments.Count);
            Assert.All(comments, c => Assert.Equal(123, c.MovieId));
        }
    }
}
