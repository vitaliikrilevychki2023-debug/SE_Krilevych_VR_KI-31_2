using System.Threading.Tasks;
using System.Windows;

namespace lab2_14.api.Post;

public class AddReview
{
    public static async Task<bool> AddReviewRequestAsync(string name, string text, int rating, int serviceId)
    {
        await Task.Delay(100);

        if (rating < 1 || rating > 5)
        {
            MessageBox.Show("Рейтинг має бути від 1 до 5.");
            return false;
        }

        MessageBox.Show("Відгук успішно додано.");
        return true;
    }
}