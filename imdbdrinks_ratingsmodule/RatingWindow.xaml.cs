using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace imdbdrinks_ratingsmodule
{
    public sealed partial class RatingWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Bottle> Bottles { get; set; }
        public int ratingScore { get; set; }

        private string emptyBottlePath = "ms-appx:///Assets/Bottle.png";
        private string filledBottlePath = "ms-appx:///Assets/FullBottle.png";

        public event PropertyChangedEventHandler PropertyChanged;
        private RatingViewModel _ratingViewModel;

        public RatingWindow(RatingViewModel viewModel)
        {
            this.InitializeComponent();
            Bottles = new ObservableCollection<Bottle>();

            // Initialize 5 bottles in an empty state
            for (int i = 0; i < 5; i++)
            {
                Bottles.Add(new Bottle { ImageSource = emptyBottlePath });
            }

            rootGrid.DataContext = this;
            _ratingViewModel = viewModel;
        }

        private void Bottle_Click(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image img && img.DataContext is Bottle bottle)
            {
                int index = Bottles.IndexOf(bottle);

                for (int i = 0; i < Bottles.Count; i++)
                {
                    Bottles[i].ImageSource = i <= index ? filledBottlePath : emptyBottlePath;
                }
                ratingScore = index + 1;
            }
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            // If no bottle was clicked, ratingScore will be 0. Exit the method to prevent an error.
            if (ratingScore == 0)
            {
                // Optionally, show a message to the user here.
                return;
            }

            Rating rating = new Rating();
            rating.ProductId = 100; // mock value, should be replaced with actual product id
            rating.RatingValue = ratingScore;
            rating.UserId = _ratingViewModel.Ratings.Count + 1; // mock value, should be replaced with actual user id

            _ratingViewModel.AddRating(rating);
            this.Close();
        }
    }

    public class Bottle : INotifyPropertyChanged
    {
        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
