using System.Windows;
using lab2_14.api.Get;
using lab2_14.api.Post;
using lab2_14.Entity;

namespace lab2_14.Pages;

public partial class Bonuses : Window
{
    private UserScore _userScore {get; set;}
    public Bonuses()
    {
        InitializeComponent();
        _userScore = new UserScore();
        DataContext = _userScore;
    }

    private async void GetBonusesClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Console.WriteLine(_userScore.Name);
            if (string.IsNullOrWhiteSpace(_userScore.Name))
            {
                MessageBox.Show($"Введіть ім'я користувача");
                return;
            }
            var (success, userScore) = await GetBonuses.GetBonusesResponseAsync(_userScore.Name);
            Console.WriteLine($"{success}, {userScore.Name}, {userScore.Score}");
            if (success)
            {
                MessageBox.Show($"Кількість бонусів: {userScore.Score}", "Бонуси");
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка під час отримання кількості балів: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async void SetZeroBonusesClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success = await ResetScore.ResetScoreRequestAsync(_userScore.Name);
            if (success)
            {
                MessageBox.Show($"Бонуси були обнулені для користувача {_userScore.Name}", "Бонуси");
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка під час обнулення балів: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}