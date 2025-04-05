using System;

namespace imdbdrinks_ratingsmodule.Domain
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int ProductId { get; set; } // Reference to the product (for future integration)
        public int UserId { get; set; }
        public double RatingValue { get; set; } // 1 to 5 stars
        public DateTime RatingDate { get; set; }
        public bool IsActive { get; set; }

        // Validate the rating is between 1 and 5.
        public bool IsValid() => RatingValue >= 1 && RatingValue <= 5;
    }
}
