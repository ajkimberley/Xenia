using ErrorOr;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Xenia.Application.Commands;
using Xenia.Application.Dtos;
using Xenia.Application.Queries;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Persistence;
using Xenia.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddDbContext<BookingContext>((container, options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Composition Root
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

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
    _ = hotelContext?.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Partial Program class added to support integration testing
public partial class Program
{
}