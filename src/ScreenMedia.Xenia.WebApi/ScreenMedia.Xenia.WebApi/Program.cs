using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.HotelManagement.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddDbContext<HotelManagementContext>((container, options) =>
    _ = options.UseSqlServer("Server=localhost;Database=Xenia;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();

    // For initial dev purposes only
    // TODO: Replace with migrations
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetService<HotelManagementContext>();
    _ = (context?.Database.EnsureCreated());
}

_ = app.UseHttpsRedirection();

_ = app.MapControllers();

app.Run();

// Partial Program class added to support integration testing
public partial class Program { }