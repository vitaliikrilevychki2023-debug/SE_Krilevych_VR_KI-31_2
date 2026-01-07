using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class UpdateService
{
    public static async Task<bool> UpdateServiceRequestAsync(string name, int price, int serviceId)
    {
        await Task.Delay(100);

        if (price <= 0)
        {
            MessageBox.Show("Ціна має бути більшою за 0.");
            return false;
        }

        MessageBox.Show("Сервіс успішно оновлено.");
        return true;
    }
}