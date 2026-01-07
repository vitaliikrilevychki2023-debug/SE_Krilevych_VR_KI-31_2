using lab3_14.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_14.api.Services;

public class AppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAppointment(string clientName, DateTime appointmentTimeInput, string phoneNumber, int serviceId)
    {
        try
        {
            // Перевірка на пустоту імені клієнта
            if (string.IsNullOrWhiteSpace(clientName))
            {
                throw new ArgumentException("Client name cannot be empty or whitespace.");
            }

            // Перевірка на мінімальну довжину імені клієнта
            if (clientName.Length < 3)
            {
                throw new ArgumentException("Client name must be at least 3 characters long.");
            }

            // Перевірка на пустоту номера телефону
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be empty or whitespace.");
            }

            // Перевірка формату номера телефону (наприклад, для довжини 10-15 символів)
            if (phoneNumber.Length < 10 || phoneNumber.Length > 15)
            {
                throw new ArgumentException("Phone number must be between 10 and 15 characters.");
            }

            // Перевірка, що час зустрічі є майбутньою датою
            if (appointmentTimeInput <= DateTime.Now)
            {
                throw new ArgumentException("Appointment time must be a future date.");
            }

            // Перевірка на існування послуги
            var existingService = await _context.Services.FindAsync(serviceId);
            if (existingService == null)
            {
                throw new InvalidOperationException("Service does not exist.");
            }

            // Створення нового запису зустрічі
            var newAppointment = new Appointment()
            {
                Name = clientName,
                Time = appointmentTimeInput,
                Phone = phoneNumber,
                ServiceId = serviceId
            };

            _context.Appointments.Add(newAppointment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }



    public async Task<bool> UpdateAppointment(string clientName, DateTime appointmentTime, string phoneNumber, int appointmentId)
    {
        try
        {
            
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
            
            if (existingAppointment == null)
            {
                throw new InvalidOperationException("Appointment does not exist.");
            }

            // Оновити властивості існуючої зустрічі
            existingAppointment.Name = clientName;
            existingAppointment.Time = appointmentTime;
            existingAppointment.Phone = phoneNumber;

            // Зберегти зміни
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAppointment(int appointmentId, int amount, string name)
    {
        try
        {
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (existingAppointment == null)
            {
                throw new InvalidOperationException("Appointment does not exist.");
            }

            var userScore = await _context.UserScores.FirstOrDefaultAsync(u => u.Name == name);

            // Якщо запис користувача не знайдено, створюємо новий
            if (userScore == null)
            {
                userScore = new UserScore
                {
                    Name = name,
                    Score = 0 // Початковий рахунок
                };
                await _context.UserScores.AddAsync(userScore);
            }

            // Видаляємо зустріч
            _context.Appointments.Remove(existingAppointment);

            // Оновлюємо бали користувача
            userScore.Score += amount;

            // Зберігаємо зміни в базі даних
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }


    public async Task<(bool, List<Appointment>)> GetAppointments()
    {
        try
        {
            var appointments = await _context.Appointments.ToListAsync();
            return (true, appointments);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }
}
