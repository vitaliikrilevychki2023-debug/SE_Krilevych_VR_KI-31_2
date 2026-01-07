namespace lab2_14.Entity;

public class Review
{
    public string Text { get; set; }
    public int Rating { get; set; }

    public Review(string text)
    {
        Text = text;
    }
}