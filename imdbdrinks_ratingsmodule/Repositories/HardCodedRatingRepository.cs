using System;
using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public class HardCodedRatingRepository : IRatingRepository
    {
        private readonly List<Rating> _ratings = new List<Rating>();

        public HardCodedRatingRepository()
        {
            // Hard-coded sample data.
            _ratings.Add(new Rating
            {
                RatingId = 1,
                ProductId = 100,
                UserId = 1,
                RatingValue = 4,
                RatingDate = DateTime.Now.AddDays(-2),
                IsActive = true
            });
            _ratings.Add(new Rating
            {
                RatingId = 2,
                ProductId = 100,
                UserId = 2,
                RatingValue = 5,
                RatingDate = DateTime.Now.AddDays(-1),
                IsActive = true
            });
        }

        public Rating FindById(long ratingId) =>
            _ratings.FirstOrDefault(r => r.RatingId == ratingId);

        public IEnumerable<Rating> FindAll() => _ratings;

        public IEnumerable<Rating> FindByProductId(long productId) =>
            _ratings.Where(r => r.ProductId == productId);

        public Rating Save(Rating rating)
        {
            var existing = _ratings.FirstOrDefault(r => r.RatingId == rating.RatingId);
            if (existing != null)
            {
                existing.ProductId = rating.ProductId;
                existing.UserId = rating.UserId;
                existing.RatingValue = rating.RatingValue;
                existing.RatingDate = rating.RatingDate;
                existing.IsActive = rating.IsActive;
            }
            else
            {
                rating.RatingId = _ratings.Count + 1;
                _ratings.Add(rating);
            }
            return rating;
        }

        public void Delete(long ratingId)
        {
            var rating = _ratings.FirstOrDefault(r => r.RatingId == ratingId);
            if (rating != null)
                rating.IsActive = false;
        }
    }
}
