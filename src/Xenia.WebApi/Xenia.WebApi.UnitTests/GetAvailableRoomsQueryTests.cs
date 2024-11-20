using Xenia.Application.Dtos;
using Xenia.Application.Queries;
using Xenia.Bookings.Domain.Entities;
using Xenia.WebApi.Commands.UnitTests.Fakes;

namespace Xenia.WebApi.Commands.UnitTests;

public class GetAvailableRoomsQueryTests
{
    private readonly GetAvailableRoomsHandler _sut;
    private readonly FakeUow _uow;

    public GetAvailableRoomsQueryTests()
    {
        _uow = new FakeUow();
        _sut = new GetAvailableRoomsHandler(_uow.Hotels);
    }

    [Fact]
    public async Task Given_HotelHasAvailableRooms_Should_ReturnAvailableRooms()
    {
        var dto = new HotelDto("Holiday Bin");
        var hotel = Hotel.Create(dto.Name);

        var existingFrom = new DateTime(2024, 04, 15);
        var existingTo = new DateTime(2024, 04, 20);

        hotel.BookRoom("Foo", "foo@bar.com", existingFrom, existingTo, "single");
        
        await _uow.Hotels.AddAsync(hotel);

        var queryFrom = new DateTime(2024, 04, 15);
        var queryTo = new DateTime(2024, 04, 20);
        
        var qry = new GetAvailableRoomsQuery(hotel.Id, queryFrom, queryTo);
        var actual = await _sut.Handle(qry, CancellationToken.None);
        
        Assert.True(!actual.IsError);
        Assert.Equal(5, actual.Value.Count);
    }
}