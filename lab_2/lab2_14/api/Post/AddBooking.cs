using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class AddBooking
{
    public static async Task<bool> AddBookingRequestAsync(string name, string phone, string time, int serviceId)
    {
        await Task.Delay(100);

        if (string.IsNullOrWhiteSpace(name) || serviceId <= 0)
        {
            MessageBox.Show("Некоректні дані бронювання.");
            return false;
        }

        MessageBox.Show("Бронювання успішно додано.");
        return true;
    }
}