using System.Windows;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages;

public partial class SignUpForService : Window
{
    public Service Service { get; set; }
    public Booking Form { get; set; }
    
    public SignUpForService(Service service)
    {
        InitializeComponent();
        Form = new Booking(string.Empty, string.Empty, string.Empty);
        Service = service;
        DataContext = Form;
    }

    private async void SignUpClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success = await AddBooking.AddBookingRequestAsync(Form.Name, Form.Phone, Form.Time, Service.Id);
            var user = await CreateUser.CreateUserRequestAsync(Form.Name);
            if (success)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка під час запису на послугу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}