using BlogASP.Models;

namespace BlogASP.Repository
{
    public interface IRatingService
    {
        void AddRating(string username, int articleId);
        void RemoveRating(string username, int articleId);
        int GetRatingCount(int articleId);
        RatingModel GetRating(string username, int articleId);
        bool HasStarred(string username, int articleId);

    }
}
