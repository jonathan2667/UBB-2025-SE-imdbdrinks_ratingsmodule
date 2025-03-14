using System;

namespace imdbdrinks_ratingsmodule.Domain
{
    public class Rating
    {
        public long RatingId { get; set; }
        public long ProductId { get; set; } // Reference to the product (for future integration)
        public long UserId { get; set; }
        public int RatingValue { get; set; } // 1 to 5 stars
        public DateTime RatingDate { get; set; }
        public bool IsActive { get; set; }

        // Validate the rating is between 1 and 5.
        public bool IsValid() => RatingValue >= 1 && RatingValue <= 5;
    }
}
