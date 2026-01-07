using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using lab3_14.api.Services;

public class TcpServer
{
    private readonly ServiceService _serviceService;
    private readonly AppointmentService _appointmentService;
    private readonly ReviewService _reviewService;
    private readonly UserScoreService _userScoreService;
    private readonly ILogger _logger;

    public TcpServer(ServiceService serviceService,
        AppointmentService appointmentService,
        ReviewService reviewService,
        UserScoreService userScoreService,
        ILogger<TcpServer> logger)
    {
        _serviceService = serviceService;
        _appointmentService = appointmentService;
        _reviewService = reviewService;
        _userScoreService = userScoreService; 
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (client)
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine(request);

            try
            {
                // Десеріалізація запиту
                var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(request);
                if (requestData == null)
                {
                    var response = new
                    {
                        success = false,
                        message = "Invalid command"
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                    return;
                }

                var command = requestData["command"].ToLower();
                if (command == "createservice")
                {
                    // Отримання назви, назви категорії та місця розташування з запиту
                    string serviseName = requestData["Name"];
                    int price = int.Parse(requestData["Price"]);

                    bool createSuccess = await _serviceService.CreateService(serviseName, price);
                    var response = new
                    {
                        success = createSuccess.ToString(),
                        message = createSuccess ? "Service created successfully." : "Invalid Name."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "editservice")
                {
                    // New edit service logic
                    int serviceId = int.Parse(requestData["ServiceId"]);
                    string serviceName = requestData["Name"];
                    int price = int.Parse(requestData["Price"]);

                    bool editSuccess = await _serviceService.EditService(serviceId, serviceName, price);
                    var response = new
                    {
                        success = editSuccess,
                        message = editSuccess ? "Service updated successfully." : "Failed to update service."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getservices")
                {
                    var (getSuccess, services) = await _serviceService.GetServices();
                    var response = new
                    {
                        success = getSuccess.ToString(),
                        message = services
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "createappointment")
                {
                    string clientName = requestData["Name"];
                    string phoneNumber = requestData["Phone"];
                    DateTime appointmentTime = DateTime.Parse(requestData["Time"]);
                    int serviceId = int.Parse(requestData["ServiceId"]);

                    bool createSuccess =
                        await _appointmentService.CreateAppointment(clientName, appointmentTime, phoneNumber,
                            serviceId);
                    var response = new
                    {
                        success = createSuccess,
                        message = createSuccess ? "Appointment created successfully." : "Failed to create appointment."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "updateappointment")
                {
                    string clientName = requestData["Name"];
                    string phoneNumber = requestData["Phone"];
                    DateTime appointmentTime = DateTime.Parse(requestData["Time"]);
                    int appointmentId = int.Parse(requestData["AppointmentId"]);

                    bool createSuccess =
                        await _appointmentService.UpdateAppointment(clientName, appointmentTime, phoneNumber, appointmentId);
                    var response = new
                    {
                        success = createSuccess,
                        message = createSuccess ? "Appointment update successfully." : "Failed to update appointment."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "deleteappointment")
                {
                    int appointmentId = int.Parse(requestData["AppointmentId"]);
                    int amount = int.Parse(requestData["Amount"]);
                    string clientName = requestData["Name"];

                    bool createSuccess =
                        await _appointmentService.DeleteAppointment(appointmentId, amount, clientName);
                    var response = new
                    {
                        success = createSuccess,
                        message = createSuccess ? "Appointment delete successfully." : "Failed to delete appointment."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getappointments")
                {
                    var (getSuccess, appointments) = await _appointmentService.GetAppointments();
                    var response = new
                    {
                        success = getSuccess.ToString(),
                        message = appointments
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "createreview")
                {
                    string reviewerName = requestData["Name"];
                    string reviewText = requestData["Text"];
                    int rating = int.Parse(requestData["Rating"]);
                    int serviceId = int.Parse(requestData["ServiceId"]);

                    bool createSuccess = await _reviewService.CreateReview(reviewerName, reviewText, rating, serviceId);
                    var response = new
                    {
                        success = createSuccess,
                        message = createSuccess ? "Review created successfully." : "Failed to create review."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getreviews")
                {
                    int serviceId = int.Parse(requestData["ServiceId"]); // New line to get the service ID
                    var (getSuccess, reviews) = await _reviewService.GetReviews(serviceId); // Pass the service ID
                    var response = new
                    {
                        success = getSuccess.ToString(),
                        message = reviews
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "adduser")
                {
                    string userName = requestData["Name"]; // Get the name from request
                    bool addUserSuccess = await _userScoreService.AddUser(userName);
                    var response = new
                    {
                        success = addUserSuccess,
                        message = addUserSuccess ? "User added successfully." : "Failed to add user."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "addscore")
                {
                    string userName = requestData["Name"]; // Get the name from request
                    int points = int.Parse(requestData["Points"]); // Get points from request
                    bool addScoreSuccess = await _userScoreService.AddScore(userName, points);
                    var response = new
                    {
                        success = addScoreSuccess.ToString(),
                        message = addScoreSuccess ? "Score added successfully." : "Failed to add score."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "resetscore")
                {
                    string userName = requestData["Name"];
                    bool resetScoreSuccess = await _userScoreService.ResetScore(userName);
                    var response = new
                    {
                        success = resetScoreSuccess,
                        message = resetScoreSuccess ? "Score reset successfully." : "Failed to reset score."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getuser")
                {
                    string userName = requestData["Name"];
                    var (getSuccess, userScore) = await _userScoreService.GetUser(userName);
                    var response = new
                    {
                        success = getSuccess,
                        userScore = userScore
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else
                {
                    var response = new
                    {
                        success = false,
                        message = "Invalid command"
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    success = false,
                    message = ex.Message
                };
                var jsonResponse = JsonSerializer.Serialize(response);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
            }
        }
    }
}