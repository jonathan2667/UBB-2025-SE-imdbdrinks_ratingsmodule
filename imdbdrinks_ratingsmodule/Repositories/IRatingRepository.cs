using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public interface IRatingRepository
    {
        Rating FindById(long ratingId);
        IEnumerable<Rating> FindAll();
        IEnumerable<Rating> FindByProductId(long productId);
        Rating Save(Rating rating);
        void Delete(long ratingId);
    }
}
