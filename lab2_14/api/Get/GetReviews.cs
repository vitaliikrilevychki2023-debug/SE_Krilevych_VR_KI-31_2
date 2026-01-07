using System.Collections.ObjectModel;
using System.Threading.Tasks;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetReviews
{
    public static async Task<ObservableCollection<Review>> GetReviewsResponseAsync(int serviceId)
    {
        await Task.Delay(100);

        return new ObservableCollection<Review>
        {
            new Review("Чудовий сервіс!") { Rating = 5 },
            new Review("Дуже приємний персонал") { Rating = 5 },
            new Review("Все швидко і якісно") { Rating = 4 },
            new Review("Нормально, без зауважень") { Rating = 4 },
            new Review("Очікував більшого") { Rating = 3 },
            new Review("Не сподобалось") { Rating = 2 }
        };
    }
}