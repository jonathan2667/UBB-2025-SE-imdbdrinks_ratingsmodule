using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels
{
    public class RatingViewModel : INotifyPropertyChanged
    {
        private readonly RatingService _ratingService;
        private ObservableCollection<Rating> _ratings;

        public ObservableCollection<Rating> Ratings
        {
            get => _ratings;
            set
            {
                if (_ratings != value)
                {
                    _ratings = value;
                    OnPropertyChanged();
                }
            }
        }

        private Rating _selectedRating;
        public Rating SelectedRating
        {
            get => _selectedRating;
            set
            {
                // Check for a change to avoid infinite loops from TwoWay binding
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _averageRating;
        public double AverageRating
        {
            get => _averageRating;
            set
            {
                if (_averageRating != value)
                {
                    _averageRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public RatingViewModel(RatingService ratingService)
        {
            _ratingService = ratingService;
            Ratings = new ObservableCollection<Rating>();
        }

        public void LoadRatingsForProduct(long productId)
        {
            var ratings = _ratingService.GetRatingsByProduct(productId);
            Ratings.Clear();
            foreach (var rating in ratings)
            {
                Ratings.Add(rating);
            }
            AverageRating = _ratingService.GetAverageRating(productId);
        }

        public void AddRating(Rating rating)
        {
            _ratingService.CreateRating(rating);
            LoadRatingsForProduct(rating.ProductId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
