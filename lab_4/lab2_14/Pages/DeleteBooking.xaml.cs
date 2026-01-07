using System.ComponentModel;
using System.Windows;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages
{
    public partial class DeleteBooking : Window, INotifyPropertyChanged
    {
        private int _amount;
        public Booking Form { get; set; }

        public int amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(amount));
            }
        }

        public DeleteBooking(Booking form)
        {
            InitializeComponent();
            Form = form;
            DataContext = this;
        }

        private async void DeleteBookingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine(amount);
                Console.WriteLine(Form.Id);
                var success = await DeleteBookingRequest.DeleteBookingRequestAsync(Form.Id, amount, Form.Name);
                if (success)
                {
                    MessageBox.Show("Booking Deleted");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка під час запису на послугу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}