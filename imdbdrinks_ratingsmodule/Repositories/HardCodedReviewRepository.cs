using System;
using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public class HardCodedReviewRepository : IReviewRepository
    {
        private readonly List<Review> _reviews = new List<Review>();

        public HardCodedReviewRepository()
        {
            // Hard-coded sample data.
            _reviews.Add(new Review
            {
                ReviewId = 1,
                RatingId = 1,
                UserId = 1,
                Content = "Great drink, really refreshing!",
                CreationDate = DateTime.Now.AddDays(-2),
                IsActive = true
            });
            _reviews.Add(new Review
            {
                ReviewId = 2,
                RatingId = 2,
                UserId = 2,
                Content = "Amazing flavor, would drink again.",
                CreationDate = DateTime.Now.AddDays(-1),
                IsActive = true
            });
        }

        public Review FindById(long reviewId) =>
            _reviews.FirstOrDefault(r => r.ReviewId == reviewId);

        public IEnumerable<Review> FindAll() => _reviews;

        public IEnumerable<Review> FindByRatingId(long ratingId) =>
            _reviews.Where(r => r.RatingId == ratingId);

        public Review Save(Review review)
        {
            var existing = _reviews.FirstOrDefault(r => r.ReviewId == review.ReviewId);
            if (existing != null)
            {
                existing.RatingId = review.RatingId;
                existing.UserId = review.UserId;
                existing.Content = review.Content;
                existing.CreationDate = review.CreationDate;
                existing.IsActive = review.IsActive;
            }
            else
            {
                review.ReviewId = _reviews.Count + 1;
                _reviews.Add(review);
            }
            return review;
        }

        public void Delete(long reviewId)
        {
            var review = _reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            if (review != null)
                review.IsActive = false;
        }
    }
}
