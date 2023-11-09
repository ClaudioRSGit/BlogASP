using BlogASP.DAL;
using BlogASP.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogASP.Repository
{
    public class RatingService : IRatingService
    {
        private readonly DatabaseContext _context;

        public RatingService(DatabaseContext dbContext)
        {
            _context = dbContext;
        }

        public void AddRating(string username, int articleId)
        {
            var article = _context.Articles.Find(articleId);

            if (article != null)
            {
                // Check if the user has already rated the article
                var existingRating = _context.Ratings
                    .FirstOrDefault(r => r.ArticleId == articleId && r.Username == username);

                if (existingRating == null)
                {
                    // If the user hasn't rated, increment stars and add a new rating
                    article.Stars++;
                    _context.SaveChanges();

                    var newRating = new RatingModel
                    {
                        ArticleId = articleId,
                        Username = username,
                    };

                    _context.Ratings.Add(newRating);
                    _context.SaveChanges();
                }
                else
                {
                    // If the user has already rated, you might want to handle this case
                    // You can choose to ignore, update, or throw an exception
                    // For now, let's ignore and not update
                }
            }
        }
        public void RemoveRating(string username, int articleId)
        {
            var article = _context.Articles.Find(articleId);

            if (article != null)
            {
                // Decrement stars
                article.Stars--;
                _context.SaveChanges();

                // Remove the rating
                var existingRating = _context.Ratings
                    .FirstOrDefault(r => r.ArticleId == articleId && r.Username == username);

                if (existingRating != null)
                {
                    _context.Ratings.Remove(existingRating);
                    _context.SaveChanges();
                }
            }
        }

        public int GetRatingCount(int articleId)
        {
            // Implement your logic to get the total number of stars for an article
            // For example:
            return _context.Ratings.Count(r => r.ArticleId == articleId);
        }

        public RatingModel GetRating(string username, int articleId)
        {
            // Implement your logic to get a specific rating by username and articleId
            // For example:
            return _context.Ratings
                .FirstOrDefault(r => r.ArticleId == articleId && r.Username == username);
        }
        public bool HasStarred(string username, int articleId)
        {
            // Check if there is any rating for the given user and article
            return _context.Ratings.Any(r => r.Username == username && r.ArticleId == articleId);
        }
    }
}
