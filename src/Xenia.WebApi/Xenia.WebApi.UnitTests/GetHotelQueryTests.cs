using ErrorOr;

using Xenia.Application.HotelManagement;
using Xenia.Bookings.Domain.Hotels;
using Xenia.WebApi.Commands.UnitTests.Fakes;

namespace Xenia.WebApi.Commands.UnitTests;

public class GetHotelQueryTests
{
    private readonly GetHotelHandler _sut;
    private readonly FakeUow _uow;

    public static IEnumerable<object[]> HotelDtoTestData()
    {
        yield return [new HotelDto("Travel Bodge")];
        yield return [new HotelDto("Mediocre Inn")];
        yield return [new HotelDto("Holiday Bin")];
    }

    public GetHotelQueryTests()
    {
        _uow = new FakeUow();
        _sut = new GetHotelHandler(_uow.Hotels);
    }

    [Theory]
    [MemberData(nameof(HotelDtoTestData))]
    public async Task Given_HotelInRepo_Should_ReturnCorrectHotel(HotelDto dto)
    {
        var newHotel = Hotel.Create(dto.Name);
        await _uow.Hotels.AddAsync(newHotel);

        var qry = new GetHotelQuery(newHotel.Id);
        var actual = await _sut.Handle(qry, CancellationToken.None);

        var expected = dto with { Id = newHotel.Id };
        Assert.True(!actual.IsError);
        Assert.Equivalent(expected, actual.Value);
    }

    [Fact]
    public async Task Given_HotelNotInRepo_Should_ThrowResourceNotFoundException()
    {
        var qry = new GetHotelQuery(Guid.NewGuid());
        var actual = await _sut.Handle(qry, CancellationToken.None);

        var expected = Error.NotFound();
        Assert.True(actual.IsError);
        Assert.Equal(expected, actual.FirstError);
    }
}