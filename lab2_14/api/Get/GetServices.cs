using System.Collections.ObjectModel;
using System.Threading.Tasks;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetServices
{
    public static async Task<ObservableCollection<Service>> GetServicesResponseAsync()
    {
        await Task.Delay(100);

        return new ObservableCollection<Service>
        {
            new Service("Стрижка", 300),
            new Service("Фарбування", 800),
            new Service("Укладка", 250),
            new Service("Миття голови", 150),
            new Service("Борода", 200),
            new Service("Комплекс (стрижка + борода)", 450),
            new Service("Відновлення волосся", 600)
        };
    }
}