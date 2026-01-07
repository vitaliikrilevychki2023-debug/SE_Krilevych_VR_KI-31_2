using System.Threading.Tasks;
using System.Windows;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetBonuses
{
    public static async Task<(bool, UserScore)> GetBonusesResponseAsync(string name)
    {
        await Task.Delay(100);

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Некоректне імʼя користувача.");
            return (false, new UserScore());
        }

        return (true, new UserScore
        {
            Id = 1,
            Name = name,
            Score = 20
        });
    }
}