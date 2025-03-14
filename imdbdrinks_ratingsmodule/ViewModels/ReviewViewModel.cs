using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels
{
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private readonly ReviewService _reviewService;
        private ObservableCollection<Review> _reviews;

        public ObservableCollection<Review> Reviews
        {
            get => _reviews;
            set { _reviews = value; OnPropertyChanged(); }
        }

        private Review _selectedReview;
        public Review SelectedReview
        {
            get => _selectedReview;
            set { _selectedReview = value; OnPropertyChanged(); }
        }

        public ReviewViewModel(ReviewService reviewService)
        {
            _reviewService = reviewService;
            Reviews = new ObservableCollection<Review>();
        }

        public void LoadReviewsForRating(long ratingId)
        {
            var reviews = _reviewService.GetReviewsByRating(ratingId);
            Reviews.Clear();
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }
        }

        public void AddReview(Review review)
        {
            _reviewService.CreateReview(review);
            LoadReviewsForRating(review.RatingId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
