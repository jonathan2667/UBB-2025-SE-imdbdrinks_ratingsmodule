using Microsoft.UI.Xaml;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain;  // Needed for creating new Review objects

namespace imdbdrinks_ratingsmodule
{
    public sealed partial class MainWindow : Window
    {
        // Public properties for binding.
        public RatingViewModel ViewModel { get; set; }
        public ReviewViewModel ReviewVM { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            // Create repository instances.
            var ratingRepo = new HardCodedRatingRepository();
            var reviewRepo = new HardCodedReviewRepository();

            // Create service instances.
            var ratingService = new RatingService(ratingRepo);
            var reviewService = new ReviewService(reviewRepo);

            // Instantiate ViewModels.
            ViewModel = new RatingViewModel(ratingService);
            ReviewVM = new ReviewViewModel(reviewService);

            // Load ratings for product ID 100.
            ViewModel.LoadRatingsForProduct(100);
            if (ViewModel.Ratings.Count > 0)
            {
                ViewModel.SelectedRating = ViewModel.Ratings[0];
                ReviewVM.LoadReviewsForRating(ViewModel.SelectedRating.RatingId);
            }
        }

        private void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a rating is selected.
            if (ViewModel.SelectedRating == null)
                return;

            // Get the review text.
            string content = ReviewTextBox.Text;
            if (!string.IsNullOrWhiteSpace(content))
            {
                // Create a new review. (Using dummy UserId 999 for this example.)
                var newReview = new Review
                {
                    RatingId = ViewModel.SelectedRating.RatingId,
                    UserId = 999,
                    Content = content,
                    IsActive = true
                    // CreationDate will be set in the service.
                };

                // Add the review via the ViewModel.
                ReviewVM.AddReview(newReview);

                // Clear the TextBox.
                ReviewTextBox.Text = string.Empty;
            }
        }
    }
}
