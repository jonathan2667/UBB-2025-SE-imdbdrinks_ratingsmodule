using Microsoft.UI.Xaml;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain; 
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

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            var reviewWindow = new ReviewWindow(ViewModel, ReviewVM);
            reviewWindow.Activate();

        }
       
    }
}
