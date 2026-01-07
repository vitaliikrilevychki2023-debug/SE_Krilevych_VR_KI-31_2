using lab3_14;
using lab3_14.api;
using lab3_14.api.Models;
using lab3_14.api.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<TcpServer>();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<UserScoreService>();

builder.Services.AddScoped<Worker>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Ініціалізація бази даних
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

await host.RunAsync();