using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class EditBookingRequest
{
    public static async Task<bool> EditBookingRequestRequestAsync(
        string name,
        string phone,
        string time,
        int bookingId)
    {
        await Task.Delay(100);
        
        MessageBox.Show("Бронювання успішно змінено.");
        return true;
    }
}