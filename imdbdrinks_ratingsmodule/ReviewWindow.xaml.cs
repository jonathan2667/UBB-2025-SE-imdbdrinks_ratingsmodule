using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace imdbdrinks_ratingsmodule
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewWindow : Window
    {

        public RatingViewModel ViewModel { get; set; }
        public ReviewViewModel ReviewVM { get; set; }

        public ReviewWindow(RatingViewModel viewModel, ReviewViewModel reviewVM)
        {
            this.InitializeComponent();
            ViewModel = viewModel;
            ReviewVM = reviewVM;
        }

        private void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            // Get the review text.
            string content = ReviewTextBox.Text;
            if (!string.IsNullOrWhiteSpace(content))
            {
                // Determine the rating ID.
                // If no rating is selected, use a default value (e.g., 0).
                long ratingId = ViewModel.SelectedRating != null ? ViewModel.SelectedRating.RatingId : 0;

                // Create a new review.
                var newReview = new Review
                {
                    RatingId = ratingId,
                    UserId = 999, // Update with the actual user id if needed.
                    Content = content,
                    IsActive = true
                    // CreationDate can be set here or in the service layer.
                };

                // Add the review via the Review ViewModel.
                ReviewVM.AddReview(newReview);

                // Clear the TextBox.
                ReviewTextBox.Text = string.Empty;
            }
        }


        private void GenerateAIReview_Click(object sender, RoutedEventArgs e)
        {
            string aiGeneratedReview = "This is an AI-generated review based on your input.";

            var aiReviewWindow = new AIReviewWindow(OnAIReviewGenerated);
            aiReviewWindow.Activate();
        }

        private void OnAIReviewGenerated(string aiReview)
        {
            ReviewTextBox.Text = aiReview;
        }
    }
}