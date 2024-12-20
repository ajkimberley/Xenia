using Common.Domain;

using ErrorOr;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Modules.Bookings.Application.BookRoom;
using Modules.Bookings.Domain;
using Modules.Bookings.Persistence;
using Modules.Bookings.Persistence.Repositories;
using Modules.HotelAdmin.Application;
using Modules.HotelAdmin.Application.GetAvailableRooms;
using Modules.HotelAdmin.Domain.Availabilities;
using Modules.HotelAdmin.Domain.Hotels;
using Modules.HotelAdmin.Persistence.Repositories;

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