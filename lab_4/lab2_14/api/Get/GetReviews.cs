using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab2_14.Entity;

namespace lab2_14.api.Get;

public class GetReviews
{
    private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для виконання запиту на отримання працівників
        public static async Task<ObservableCollection<Review>> GetReviewsResponseAsync(int ServiceId)
        {
            var model = new
            {
                command = "getreviews",
                ServiceId = ServiceId.ToString()
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
                    try
                    {
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject?.message != null)
                        {
                            return new ObservableCollection<Review>(responseObject.message);
                        }
                        else
                        {
                            return new ObservableCollection<Review>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Error parsing response: {ex.Message}");
                        return new ObservableCollection<Review>();
                    }
                }
            }
        }
        private class ResponseWrapper
        {
            public List<Review> message { get; set; }
        }
}