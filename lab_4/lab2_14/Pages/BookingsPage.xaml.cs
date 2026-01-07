using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lab2_14.api.Get;
using lab2_14.Entity;

namespace lab2_14.Pages;

public partial class BookingsPage : Window
{
    public ObservableCollection<Booking> Bookings { get; set; }
    public Button? _currentSelectedButton;
    public Booking? _selectedService;

    public BookingsPage()
    {
        InitializeComponent();
        Bookings = new ObservableCollection<Booking>();
        DataContext = this;
        GetBookingsFromServer();
    }

    private async void GetBookingsFromServer()
    {
        try
        {
            var bookings = await GetBookings.GetBookingsResponseAsync();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Console.Write(booking.Name);
                Console.Write(" ");
                Console.Write(booking.Phone);
                Console.Write(" ");
                Console.Write(booking.Time);
                Console.WriteLine();
                Bookings.Add(booking);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error fetching bookings from server: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private void SelectBooking(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Booking booking)
        {
            if (_currentSelectedButton != null)
            {
                _currentSelectedButton.Background = Brushes.Gray;
            }

            if (button != _currentSelectedButton)
            {
                button.Background = Brushes.LightGreen;
                _currentSelectedButton = button;
                _selectedService = booking;
            }
            else
            {
                _currentSelectedButton.Background = Brushes.Gray;
                _currentSelectedButton = null;
                _selectedService = null;
            }
        }
    }

    private void SignUpFormClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_selectedService != null)
            {
                MessageBox.Show("idk.", "idk");
            }
            else
            {
                throw new InvalidOperationException("Service not selected");
            }
        }
        catch (InvalidOperationException)
        {
            MessageBox.Show("Please select a booking first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
    
    private void EditBookingClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_selectedService != null)
            {
                var edit = new EditBooking(_selectedService);
                edit.Show();
            }
            else
            {
                throw new InvalidOperationException("Booking not selected");
            }
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show("Please select a booking first.", "Warning", MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void DeleteBookingClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_selectedService != null)
            {
                var del = new DeleteBooking(_selectedService);
                del.Show();
            }
            else
            {
                throw new InvalidOperationException("Booking not selected");
            }
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show("Please select a booking first.", "Warning", MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}