using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HostelRoomManagement.Data;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);

var connectionString =
builder.Configuration.GetConnectionString("HostelConnection")
?? throw new InvalidOperationException(
"Connection string 'HostelConnection' was not found.");

builder.Services.AddDbContext<HostelDbContext>(options =>
options.UseSqlServer(connectionString));

using var host = builder.Build();

Console.WriteLine("Hostel Room Management System");
Console.WriteLine("Dependency Injection configured successfully.");

