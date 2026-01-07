using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace lab2_14.api.Post;

public class CreateUser
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000; 
    
    public static async Task<bool> CreateUserRequestAsync(string Name)
    {
        var registerModel = new
        {
            command = "adduser",
            Name = Name,
        };

        var request = JsonSerializer.Serialize(registerModel);
        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                var buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(response);
                try
                {
                    var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                    if (responseObject.success)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"{responseObject.message}", "Помилка");
                        return false;
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error parsing response: {ex.Message}");
                    return false;
                }
            }
        }
    }
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}