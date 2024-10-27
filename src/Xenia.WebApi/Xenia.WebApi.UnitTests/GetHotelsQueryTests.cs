using Xenia.Application.Queries;
using Xenia.Bookings.Domain.Entities;
using Xenia.Common.Dtos;
using Xenia.WebApi.Commands.UnitTests.Fakes;

namespace Xenia.WebApi.Commands.UnitTests;
public class GetHotelsQueryTests
{
    private readonly GetHotelsHandler _sut;
    private readonly FakeUow _uow;

    private readonly List<Hotel> _seedData = new()
    {
        Hotel.Create("Travel Bodge"),
        Hotel.Create("Mediocre Inn"),
        Hotel.Create("Holiday Bin")
    };

    public GetHotelsQueryTests()
    {
        _uow = new FakeUow();
        _sut = new GetHotelsHandler(_uow);
    }

    [Fact]
    public async Task Given_NoHotelsInRepo_Should_ReturnEmptyList()
    {
        var qry = new GetHotelsQuery();

        var actual = await _sut.Handle(qry, CancellationToken.None);
        var expected = _seedData.Select(h => new HotelDto(h.Name, h.Id)).ToList();

        Assert.Empty(actual);
    }

    [Fact]
    public async Task Given_ThreeHotelsInHotelRepo_Should_ReturnThreeHotels()
    {
        await SeedDatabase();
        var qry = new GetHotelsQuery();

        var actual = await _sut.Handle(qry, CancellationToken.None);
        var expected = _seedData.Select(h => new HotelDto(h.Name, h.Id)).ToList();

        Assert.Multiple(() =>
        {
            Assert.NotNull(actual);
            Assert.Equivalent(expected, actual);
        });
    }

    private Task SeedDatabase() => Task.WhenAll(_seedData.Select(async h => await _uow.Hotels.AddAsync(h)));
}
