using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Bookings.Domain.Exceptions;
using ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
using ScreenMedia.Xenia.WebApi.Exceptions;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests;
public class BookRoomCommandTests
{
    private readonly BookRoomHandler _sut;
    private readonly FakeUow _uow;

    public BookRoomCommandTests()
    {
        _uow = new FakeUow();
        _sut = new BookRoomHandler(_uow);
    }

    [Fact]
    public async Task Given_NoHotelsInDb_ShouldThrowException()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(Guid.NewGuid(), RoomType.Single, "Joe", "Bloggs", from, to);

        _ = await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Handle(cmd, CancellationToken.None));
    }

    [Theory]
    [InlineData(RoomType.Single)]
    [InlineData(RoomType.Double)]
    [InlineData(RoomType.Deluxe)]
    public async Task Given_NoBookingsForRoomType_ShouldSuccesfullyBook(RoomType roomType)
    {
        var hotel = Hotel.Create("Holiday Bin");
        await _uow.Hotels.AddAsync(hotel);

        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(hotel.Id, roomType, "Joe", "Bloggs", from, to);

        var response = await _sut.Handle(cmd, CancellationToken.None);

        Assert.NotNull(response);
    }

    [Theory]
    [InlineData(RoomType.Single)]
    [InlineData(RoomType.Double)]
    [InlineData(RoomType.Deluxe)]
    public async Task Given_RoomTypeFullyBooked_Exception(RoomType roomType)
    {
        var hotel = Hotel.Create("Holiday Bin");
        await _uow.Hotels.AddAsync(hotel);

        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(hotel.Id, roomType, "Joe", "Bloggs", from, to);

        _ = await _sut.Handle(cmd, CancellationToken.None);
        _ = await _sut.Handle(cmd, CancellationToken.None);

        _ = await Assert.ThrowsAsync<NoVacanciesAvailableException>(async () => await _sut.Handle(cmd, CancellationToken.None));
    }
}
