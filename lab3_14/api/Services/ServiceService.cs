using lab3_14.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_14.api.Services;

public class ServiceService
{
    private readonly AppDbContext _context;

    public ServiceService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<bool> CreateService(string name, int price)
    {
        try
        {
            // Перевірка на те, чи значення 'name' не є пустим або не складається з пробілів
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Service name cannot be empty or whitespace.");
            }

            // Перевірка на мінімальну довжину назви
            if (name.Length < 3)
            {
                throw new ArgumentException("Service name must be at least 3 characters long.");
            }

            // Перевірка на коректність ціни
            if (price <= 0)
            {
                throw new ArgumentException("Service price must be greater than zero.");
            }

            // Перевірка, чи вже існує сервіс із таким ім'ям
            var existingItem = await _context.Services.FirstOrDefaultAsync(u => u.Name == name);
            if (existingItem != null)
            {
                throw new InvalidOperationException("The service already exists.");
            }

            // Додавання нового сервісу
            var newItem = new Service()
            {
                Name = name,
                Price = price
            };

            _context.Services.Add(newItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }


    public async Task<bool> EditService(int serviceId, string name, int price)
    {
        try
        {
            // Перевірка на те, чи значення 'name' не є пустим або не складається з пробілів
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Service name cannot be empty or whitespace.");
            }

            // Перевірка на мінімальну довжину назви
            if (name.Length < 3)
            {
                throw new ArgumentException("Service name must be at least 3 characters long.");
            }

            // Перевірка на коректність ціни
            if (price <= 0)
            {
                throw new ArgumentException("Service price must be greater than zero.");
            }

            // Перевірка, чи існує сервіс із заданим serviceId
            var existingService = await _context.Services.FindAsync(serviceId);
            if (existingService == null)
            {
                throw new InvalidOperationException("Service not found.");
            }

            // Оновлення сервісу
            existingService.Name = name;
            existingService.Price = price;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }
    
    public async Task<(bool, List<Service>)> GetServices()
    {
        try
        {
            var services = await _context.Services.ToListAsync();
            return (true, services);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }
}