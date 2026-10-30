using HostelRoomManagement.Audit;
using HostelRoomManagement.Data;
using HostelRoomManagement.Menus;
using HostelRoomManagement.Repositories;
using HostelRoomManagement.Repositories.Interfaces;
using HostelRoomManagement.Services;
using HostelRoomManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create the application builder
var builder = Host.CreateApplicationBuilder(args);

// Read the connection string from appsettings.json
var connectionString =
builder.Configuration.GetConnectionString("HostelConnection")
?? throw new InvalidOperationException(
"Connection string 'HostelConnection' was not found.");

// Register Entity Framework Core and SQL Server
builder.Services.AddDbContext<HostelDbContext>(options =>
{
    options.UseSqlServer(connectionString);

    options.AddInterceptors(
        new AuditSaveChangesInterceptor());
});

// Register repositories
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Register services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// Register the main console menu
builder.Services.AddScoped<MainMenu>();

// Build the application host
using var host = builder.Build();

// Create a dependency injection scope
using var scope = host.Services.CreateScope();

// Resolve and run the main menu
var mainMenu = scope.ServiceProvider
.GetRequiredService<MainMenu>();

await mainMenu.RunAsync();
