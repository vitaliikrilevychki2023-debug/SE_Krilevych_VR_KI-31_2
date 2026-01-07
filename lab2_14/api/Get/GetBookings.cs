using System.Collections.ObjectModel;
using System.Threading.Tasks;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetBookings
{
    public static async Task<ObservableCollection<Booking>> GetBookingsResponseAsync()
    {
        await Task.Delay(100);

        return new ObservableCollection<Booking>
        {
            new Booking("Максим", "+380991234567", "10:00"),
            new Booking("Анна", "+380981112233", "11:30"),
            new Booking("Олег", "+380971234567", "13:00"),
            new Booking("Ірина", "+380931111111", "14:00"),
            new Booking("Дмитро", "+380951234123", "15:30"),
            new Booking("Марія", "+380961234567", "17:00"),
            new Booking("Андрій", "+380671234567", "18:30")
        };
    }
}