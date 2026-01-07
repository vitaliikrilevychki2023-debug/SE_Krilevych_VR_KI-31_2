using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetBonuses
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    // Метод для виконання запиту на отримання працівників
    public static async Task<(bool, UserScore)> GetBonusesResponseAsync(string Name)
    {
        var model = new
        {
            command = "getuser",
            Name = Name
        };

        var request = JsonSerializer.Serialize(model);
        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                var buffer = new List<byte>();
                var tempBuffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(tempBuffer, 0, tempBuffer.Length)) > 0)
                {
                    buffer.AddRange(tempBuffer.Take(bytesRead));
                    if (bytesRead < tempBuffer.Length)
                    {
                        break; // Закінчення повідомлення
                    }
                }

                string response = Encoding.UTF8.GetString(buffer.ToArray());
                Console.WriteLine(response);
                try
                {
                    var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                    if (responseObject.success)
                    {
                        return (true, responseObject?.userScore);
                    }
                    else
                    {
                        MessageBox.Show(responseObject.message);
                        return (false, new UserScore());
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error parsing response: {ex.Message}");
                    return (false, new UserScore());
                }
            }
        }
    }

    private class ResponseWrapper
    {
        public bool success { get; set; }
        public UserScore userScore { get; set; }
        public string message { get; set; }
    }
}

//public GetBonuses(string name)
//{
//    // Повертаємо завжди 20 незалежно від імені
//    MessageBox.Show($"Кількість балів для {name}: {20}", "Інформація", MessageBoxButton.OK,
//       MessageBoxImage.Information);
//}