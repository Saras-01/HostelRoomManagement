using HostelRoomManagement.Data;
using HostelRoomManagement.Repositories;
using HostelRoomManagement.Repositories.Interfaces;
using HostelRoomManagement.Services;
using HostelRoomManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Read the connection string from appsettings.json
var connectionString =
builder.Configuration.GetConnectionString("HostelConnection")
?? throw new InvalidOperationException(
"Connection string 'HostelConnection' was not found.");

// Register the database context
builder.Services.AddDbContext<HostelDbContext>(options =>
options.UseSqlServer(connectionString));

// Register repositories
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Register services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// Build the application host
using var host = builder.Build();

// Create a scope for resolving scoped services
using var scope = host.Services.CreateScope();

// Resolve services through dependency injection
var roomService =
scope.ServiceProvider.GetRequiredService<IRoomService>();

var studentService =
scope.ServiceProvider.GetRequiredService<IStudentService>();

// Display application status
Console.WriteLine("========================================");
Console.WriteLine("     HOSTEL ROOM MANAGEMENT SYSTEM");
Console.WriteLine("========================================");
Console.WriteLine("Room Service resolved successfully.");
Console.WriteLine("Student Service resolved successfully.");
Console.WriteLine("Dependency Injection configured successfully.");
