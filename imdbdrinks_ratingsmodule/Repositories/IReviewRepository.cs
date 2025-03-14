using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public interface IReviewRepository
    {
        Review FindById(long reviewId);
        IEnumerable<Review> FindAll();
        IEnumerable<Review> FindByRatingId(long ratingId);
        Review Save(Review review);
        void Delete(long reviewId);
    }
}
