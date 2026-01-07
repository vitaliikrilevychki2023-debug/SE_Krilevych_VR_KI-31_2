using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class CreateUser
{
    public static async Task<bool> CreateUserRequestAsync(string name)
    {
        await Task.Delay(100);

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Імʼя не може бути порожнім.");
            return false;
        }

        MessageBox.Show("Користувача успішно створено.");
        return true;
    }
}