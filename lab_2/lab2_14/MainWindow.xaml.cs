using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lab2_14.api.Get;
using lab2_14.Entity;
using lab2_14.Pages;

namespace lab2_14
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Service> Services { get; set; }
        private readonly ObservableCollection<Service> _originalServices;
        private Button? _currentSelectedButton;
        private Service? _selectedService;
        private readonly GetServices _servicesRequest;

        public MainWindow()
        {
            InitializeComponent();
            _servicesRequest = new GetServices();

            _originalServices = new ObservableCollection<Service>();
            Services = new ObservableCollection<Service>();

            DataContext = this;

            GetServicesFromServer();
        }

        private async void GetServicesFromServer()
        {
            try
            {
                var services = await GetServices.GetServicesResponseAsync();
                _originalServices.Clear();
                Services.Clear();

                foreach (var service in services)
                {
                    _originalServices.Add(service);
                    Services.Add(service);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching services from server: {ex.Message}, {ex.StackTrace}");
            }
        }

        private void SelectService(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Service service)
            {
                if (_currentSelectedButton != null)
                {
                    _currentSelectedButton.Background = Brushes.Gray;
                }

                if (button != _currentSelectedButton)
                {
                    button.Background = Brushes.LightGreen;
                    _currentSelectedButton = button;
                    _selectedService = service;
                }
                else
                {
                    _currentSelectedButton.Background = Brushes.Gray;
                    _currentSelectedButton = null;
                    _selectedService = null;
                }
            }
        }


        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox == null) return;

            string searchQuery = SearchTextBox.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchQuery))
            {
                // Якщо пошуковий запит порожній, показуємо всі сервіси
                Services.Clear();
                foreach (var service in _originalServices)
                {
                    Services.Add(service);
                }
            }
            else
            {
                // Фільтруємо сервіси
                var filteredServices = _originalServices.Where(service =>
                    service.Name.ToLower().Contains(searchQuery) ||
                    service.Price.ToString().Contains(searchQuery)
                ).ToList();

                Services.Clear();
                foreach (var service in filteredServices)
                {
                    Services.Add(service);
                }
            }
        }

        private void DetailedInfoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedService != null)
                {
                    var details = new ServiceDetails(_selectedService);
                    details.Show();
                    Close();
                }
                else
                {
                    throw new InvalidOperationException("Service not selected");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Please select a service first.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ViewingReviewsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedService != null)
                {
                    var reviews = new ViewReviews(_selectedService);
                    reviews.Show();
                    Close();
                }
                else
                {
                    throw new InvalidOperationException("Service not selected");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Please select a service first.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SignUpFormClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedService != null)
                {
                    var form = new SignUpForService(_selectedService);
                    form.Show();
                    Close();
                }
                else
                {
                    throw new InvalidOperationException("Service not selected");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Please select a service first.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BookingsClick(object sender, RoutedEventArgs e)
        {
            var bookings = new BookingsPage();
            bookings.Show();
            Close();
        }

        private void BonusClick(object sender, RoutedEventArgs e)
        {
            var bonuses = new Bonuses();
            bonuses.Show();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}