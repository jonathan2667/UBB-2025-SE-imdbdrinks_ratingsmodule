using System;

namespace imdbdrinks_ratingsmodule.Domain
{
    public class Review
    {
        public long ReviewId { get; set; }
        public long RatingId { get; set; } // The rating this review belongs to
        public long UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }

        // Validate that the review content is not empty and no longer than 500 characters.
        public bool IsValid() => !string.IsNullOrWhiteSpace(Content) && Content.Length <= 500;
    }
}
