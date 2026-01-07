using lab3_14.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_14.api.Services
{
    public class UserScoreService
    {
        private readonly AppDbContext _context;

        public UserScoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(string name)
        {
            // Перевіряємо, чи існує вже користувач з таким ім'ям
            var existingUser = await _context.UserScores
                .FirstOrDefaultAsync(u => u.Name == name);
            if (existingUser != null)
            {
                return false;
            }

            var newUser = new UserScore()
            {
                Name = name,
                Score = 0
            };

            _context.UserScores.Add(newUser);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> AddScore(string name, int points)
        {
            var user = await _context.UserScores.FirstOrDefaultAsync(u => u.Name == name);
    
            if (user == null)
            {
                await AddUser(name);
        
                user = await _context.UserScores.FirstOrDefaultAsync(u => u.Name == name);
            }

            user.Score += points;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> ResetScore(string name)
        {
            var user = await _context.UserScores.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.Score = 0; // Reset score to 0
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<(bool, UserScore)> GetUser(string name)
        {
            var user = await _context.UserScores.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            
            return (true, user);
        }
    }
}