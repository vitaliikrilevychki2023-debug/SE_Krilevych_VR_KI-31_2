using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class ResetScore
{
    public static async Task<bool> ResetScoreRequestAsync(string name)
    {
        await Task.Delay(100);

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Користувача не знайдено.");
            return false;
        }

        MessageBox.Show("Бали користувача скинуто.");
        return true;
    }
}