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

var connectionString =
builder.Configuration.GetConnectionString("HostelConnection")
?? throw new InvalidOperationException(
"Connection string 'HostelConnection' was not found.");

builder.Services.AddDbContext<HostelDbContext>(options =>
options.UseSqlServer(connectionString));

// Register repositories
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Register services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IStudentService, StudentService>();

using var host = builder.Build();

Console.WriteLine("Hostel Room Management System");
Console.WriteLine("Dependency Injection configured successfully.");
