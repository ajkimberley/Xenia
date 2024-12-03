using ErrorOr;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Xenia.Application.Bookings.BookRoom;
using Xenia.Application.HotelManagement;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Availabilities;
using Xenia.Bookings.Domain.Bookings;
using Xenia.Bookings.Domain.Hotels;
using Xenia.Bookings.Persistence;
using Xenia.Bookings.Persistence.Repositories;
using Xenia.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddDbContext<BookingContext>((container, options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Composition Root
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<Xenia.WebApi.Program>(ServiceLifetime.Singleton);

builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblyContaining<BookRoomHandler>()
        .AddValidation<GetAvailableRoomsQuery, ErrorOr<List<RoomDto>>>());

var app = builder.Build();

// TODO: Hide swagger in non-dev environment
_ = app.UseSwagger();
_ = app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // For initial dev purposes only
    // TODO: Replace with migrations
    using var scope = app.Services.CreateScope();
    using var hotelContext = scope.ServiceProvider.GetService<BookingContext>();
    _ = hotelContext?.Database.EnsureDeleted();
    _ = hotelContext?.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Partial Program class added to support integration testing
namespace Xenia.WebApi
{
    public partial class Program
    {
    }
}