using System.Windows;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages;

public partial class EditBooking : Window
{
    public Booking Form { get; set; }
    
    public EditBooking(Booking form)
    {
        InitializeComponent();
        Form = form;
        DataContext = Form;
        Console.WriteLine(Form.Id);
    }

    private async void EditBookingClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success = await EditBookingRequest.EditBookingRequestRequestAsync(Form.Name, Form.Phone, Form.Time, Form.Id);
            if (success)
            {
                MessageBox.Show("Review updated successfully");
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
}