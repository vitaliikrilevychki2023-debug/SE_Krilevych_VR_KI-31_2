using System.Threading.Tasks;
using System.Windows;
using lab2_14.Entity;

namespace lab2_14.api.Post;

public class AddStar
{
    public static async Task<bool> Send(Service service, int selectedRating)
    {
        await Task.Delay(100);

        if (selectedRating < 1 || selectedRating > 5)
        {
            MessageBox.Show("Невірна оцінка.");
            return false;
        }

        MessageBox.Show($"Ви оцінили сервіс на {selectedRating} ⭐");
        return true;
    }
}