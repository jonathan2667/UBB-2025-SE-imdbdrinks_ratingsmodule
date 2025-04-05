using Microsoft.UI.Xaml;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain;
using Microsoft.UI.Xaml.Controls;
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

            // Unique connection string for MySql database (change accordingly)
            string connection = "Server=localhost;Database=imdb;Trusted_Connection=True;TrustServerCertificate=True;";

            var reviewRepo = new DatabaseReviewRepository(connection);
            var ratingRepo = new DatabaseRatingRepository(connection);


            // Create repository instances.
            //var ratingRepo = new HardCodedRatingRepository();
            //var reviewRepo = new HardCodedReviewRepository();

            // Create service instances.
            var ratingService = new RatingService(ratingRepo);
            var reviewService = new ReviewService(reviewRepo);

            // Instantiate ViewModels.
            ViewModel = new RatingViewModel(ratingService);
            ReviewVM = new ReviewViewModel(reviewService);

            // Load ratings for product ID 100.
            ViewModel.LoadRatingsForProduct(100);
            //if (ViewModel.Ratings.Count > 0)
            //{
            //    ViewModel.SelectedRating = ViewModel.Ratings[0];
            //    ReviewVM.LoadReviewsForRating(ViewModel.SelectedRating.RatingId);
            //}
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedRating != null)
            {
                var reviewWindow = new ReviewWindow(ViewModel, ReviewVM);
                reviewWindow.Activate();
            }
            else
            {
                NoRatingSelectedDialog.ShowAsync();
            }
            return;

        }

        private void AddRating_Click(object sender, RoutedEventArgs e)
        {

            var ratingWindow = new RatingWindow(ViewModel);
            ViewModel.SelectedRating = null;
            ratingWindow.Activate();
        }
     
        private void RatingSelection_Changed(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var selectedIndex = listView.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    var selectedRating = ViewModel.Ratings[selectedIndex];
                    ViewModel.SelectedRating = selectedRating;
                    ReviewVM.LoadReviewsForRating(selectedRating.RatingId);
                }
            }
        }

    }
}
