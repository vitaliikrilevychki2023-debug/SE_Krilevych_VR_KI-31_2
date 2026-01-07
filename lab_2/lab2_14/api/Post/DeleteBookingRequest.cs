using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class DeleteBookingRequest
{
    public static async Task<bool> DeleteBookingRequestAsync(int bookingId, int amount, string name)
    {
        await Task.Delay(100);
        

        MessageBox.Show("Бронювання успішно видалено.");
        return true;
    }
}