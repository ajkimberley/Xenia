using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Queries;
using ScreenMedia.Xenia.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddDbContext<BookingContext>((container, options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Composition Root
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IValidator<GetAvailableRoomsQuery>, GetAvailableRoomsQueryValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblyContaining<Program>()
    .AddValidation<GetAvailableRoomsQuery, List<RoomDto>>());

var app = builder.Build();

// NOTE: Ordinarily I wouldn't expose Swagger in a non-dev enviornment
//       I'm doing it here for simply for testing purposes.
_ = app.UseSwagger();
_ = app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // For initial dev purposes only
    // TODO: Replace with migrations
    using var scope = app.Services.CreateScope();
    using var hotelContext = scope.ServiceProvider.GetService<BookingContext>();
    _ = hotelContext?.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Partial Program class added to support integration testing
public partial class Program { }