using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace imdbdrinks_ratingsmodule
{
    public sealed partial class RatingWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Bottle> Bottles { get; set; }
        public int ratingScore { get; set; }

        private string emptyBottlePath = "ms-appx:///Assets/bottle.png";
        private string filledBottlePath = "ms-appx:///Assets/FullBottle.png";

        public event PropertyChangedEventHandler PropertyChanged;

        public RatingWindow()
        {
            this.InitializeComponent();
            Bottles = new ObservableCollection<Bottle>();

            // Initialize 10 bottles in an empty state
            for (int i = 0; i < 5; i++)
            {
                Bottles.Add(new Bottle { ImageSource = emptyBottlePath });
            }

            rootGrid.DataContext = this;
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
                ratingScore = index;
            }
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
