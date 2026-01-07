using System.Windows;
using System.Windows.Controls;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages
{
    public partial class ServiceDetails : Window
    {
        private Service _service;

        public ServiceDetails(Service service)
        {
            InitializeComponent();
            _service = service;
            DataContext = _service;
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var success = await UpdateService.UpdateServiceRequestAsync(_service.Name, _service.Price, _service.Id);
                if (success)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Сталась помилка: {exception.Message}");
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Book Service clicked!");
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Reviews clicked!");
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}