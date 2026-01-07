using System.Windows;
using lab2_14.Entity;

namespace lab2_14.api.Post;

public class AddStar
{
    public AddStar(Service service, int selectedRating)
    {
        MessageBox.Show($"You rated {selectedRating} star(s)!", "Rating Sent", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}