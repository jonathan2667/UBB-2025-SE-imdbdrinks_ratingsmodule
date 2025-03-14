using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;

namespace imdbdrinks_ratingsmodule.Services
{
    public class RatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public Rating GetRatingById(long ratingId) =>
            _ratingRepository.FindById(ratingId);

        public IEnumerable<Rating> GetRatingsByProduct(long productId) =>
            _ratingRepository.FindByProductId(productId);

        public Rating CreateRating(Rating rating)
        {
            if (!rating.IsValid())
                throw new System.ArgumentException("Invalid rating value.");

            rating.RatingDate = System.DateTime.Now;
            rating.IsActive = true;
            return _ratingRepository.Save(rating);
        }

        public void DeleteRating(long ratingId) =>
            _ratingRepository.Delete(ratingId);

        public double GetAverageRating(long productId)
        {
            var ratings = _ratingRepository.FindByProductId(productId).Where(r => r.IsActive);
            if (!ratings.Any())
                return 0;
            return ratings.Average(r => r.RatingValue);
        }
    }
}
