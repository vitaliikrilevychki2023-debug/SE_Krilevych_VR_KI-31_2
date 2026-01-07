using lab3_14.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_14.api.Services;

public class ReviewService
{
    private readonly AppDbContext _context;
    private readonly UserScoreService _userScoreService;

    public ReviewService(AppDbContext context, UserScoreService userScoreService)
    {
        _context = context;
        _userScoreService = userScoreService;
    }

    public async Task<bool> CreateReview(string name, string text, int rating, int serviceId)
    {
        // Перевірка на пустоту імені
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty or whitespace.");
        }

        // Перевірка на мінімальну довжину імені
        if (name.Length < 3)
        {
            throw new ArgumentException("Name must be at least 3 characters long.");
        }

        // Перевірка на пустоту тексту відгуку
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Review text cannot be empty or whitespace.");
        }
        
        if (text.Length < 10)
        {
            throw new ArgumentException("Text must be at least 10 characters long.");
        }

        // Перевірка на діапазон рейтингу
        if (rating < 0 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }

        // Перевірка на існування послуги
        var existingService = await _context.Services.FindAsync(serviceId);
        if (existingService == null)
        {
            throw new InvalidOperationException("Service does not exist.");
        }

        // Створення нового відгуку
        var newReview = new Review()
        {
            Name = name,
            Text = text,
            Rating = rating,
            ServiceId = serviceId
        };

        _context.Reviews.Add(newReview);
        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            // Нарахування бонусних балів залежно від рейтингу
            int bonusPoints = rating switch
            {
                5 => 50,
                4 => 30,
                3 => 20,
                2 => 10,
                1 => 5,
                _ => 0
            };

            // Додавання балів користувачу
            await _userScoreService.AddScore(name, bonusPoints);
        }

        return result > 0;
    }


    public async Task<(bool, List<Review>)> GetReviews(int serviceId)
    {
        try
        {
            var reviews = await _context.Reviews
                .Where(r => r.ServiceId == serviceId)
                .ToListAsync();
            return (true, reviews);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }
}
