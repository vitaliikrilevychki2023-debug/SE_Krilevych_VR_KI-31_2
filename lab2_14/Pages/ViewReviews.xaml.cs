using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using lab2_14.api.Get;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages
{
    public partial class ViewReviews : Window
    {
        private int selectedRating = 0;

        public ObservableCollection<Review> Reviews { get; set; }
        public Review Review { get; set; }
        public Service Service { get; set; }
        public string Names {get; set;}

        public ViewReviews(Service service)
        {
            InitializeComponent();
            Reviews = new ObservableCollection<Review>();
            Review = new Review("");
            Service = service;
        
            DataContext = this;
            GetReviewsFromServer(service);
        }

        private async void GetReviewsFromServer(Service service)
        {
            try
            {
                var reviews = await GetReviews.GetReviewsResponseAsync(service.Id);
                Reviews.Clear();
                foreach (var review in reviews)
                {
                    Reviews.Add(review);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching reviews from server: {ex.Message}");
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button clickedStar = sender as Button;

                if (clickedStar != null)
                {
                    int starNumber = int.Parse(clickedStar.Name.Substring(4));
                    selectedRating = starNumber;
                    UpdateStarDisplay(starNumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting star: {ex.Message}");
            }
        }

        private void UpdateStarDisplay(int starNumber)
        {
            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    Button starButton = (Button)this.FindName($"Star{i}");

                    if (i <= starNumber)
                    {
                        starButton.Content = "★";
                    }
                    else
                    {
                        starButton.Content = "☆";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating star display: {ex.Message}");
            }
        }

        private async void SendReviewAndRating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Review.Text)) 
                {
                    Reviews.Add(Review);
                    Console.Write($"Review:{Names} {Review.Text}, {selectedRating}, {Service.Id}");
                    var req = await AddReview.AddReviewRequestAsync(Names, Review.Text, selectedRating, Service.Id);
                    Review.Text = "";
                }
                else
                {
                    MessageBox.Show("Please enter a review.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (selectedRating > 0)
                {
                    selectedRating = 0;
                    UpdateStarDisplay(selectedRating);
                }
                else
                {
                    MessageBox.Show("Please select a rating.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending review or rating: {ex.Message}");
            }
        }
    }
}
