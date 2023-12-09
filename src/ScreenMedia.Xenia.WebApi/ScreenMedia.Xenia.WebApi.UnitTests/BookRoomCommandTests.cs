using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;

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
    public async Task Given_NoHotelsInDb_ShouldResultInError()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(Guid.NewGuid(), RoomType.Single, "Joe", "Bloggs", from, to);

        var result = await _sut.Handle(cmd, CancellationToken.None);

        Assert.True(result.IsError);
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

        Assert.True(response.IsSuccess);
    }

    [Theory]
    [InlineData(RoomType.Single)]
    [InlineData(RoomType.Double)]
    [InlineData(RoomType.Deluxe)]
    public async Task Given_RoomTypeFullyBooked_ResultIsError(RoomType roomType)
    {
        var hotel = Hotel.Create("Holiday Bin");
        await _uow.Hotels.AddAsync(hotel);

        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(hotel.Id, roomType, "Joe", "Bloggs", from, to);

        var result1 = await _sut.Handle(cmd, CancellationToken.None);
        var result2 = await _sut.Handle(cmd, CancellationToken.None);
        var result3 = await _sut.Handle(cmd, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsSuccess);
            Assert.True(result3.IsError);
        });
    }
}