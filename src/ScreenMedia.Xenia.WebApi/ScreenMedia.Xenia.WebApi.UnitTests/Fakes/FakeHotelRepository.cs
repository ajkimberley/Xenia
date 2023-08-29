﻿using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeHotelRepository : FakeGenericRepository<Hotel>, IHotelRepository
{
    public Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        Task.FromResult(_list.Where(h => h.Name == name));
    public Task<Hotel?> GetHotelWithRoomsAndBookingsByIdAsync(Guid id) =>
        Task.FromResult(_list.Where(h => h.Id == id).SingleOrDefault());
    public Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id) =>
        Task.FromResult(_list.Where(h => h.Id == id).SingleOrDefault());
    public Task<Hotel?> GetHotelWithAvailableRooms(Guid id, DateTime? start, DateTime? to) =>
        throw new NotImplementedException();
}
