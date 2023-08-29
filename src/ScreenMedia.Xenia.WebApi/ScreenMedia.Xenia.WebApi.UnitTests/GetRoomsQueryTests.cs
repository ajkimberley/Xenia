using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
using ScreenMedia.Xenia.WebApi.Queries;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests;
public class GetRoomsQueryTests
{
    private readonly FakeUow _uow;
    private readonly GetRoomsHandler _sut;

    public GetRoomsQueryTests()
    {
        _uow = new FakeUow();
        _sut = new GetRoomsHandler(_uow);
    }

    public static IEnumerable<object[]> QueryData()
    {
        yield return new object[] { new DateTime(2024, 1, 1), new DateTime(2024, 1, 7), 5 };
        yield return new object[] { new DateTime(2024, 1, 6), new DateTime(2024, 1, 7), 5 };
        yield return new object[] { new DateTime(2024, 1, 1), new DateTime(2024, 1, 2), 5 };
        yield return new object[] { new DateTime(2024, 1, 7), new DateTime(2024, 1, 7), 6 };
        yield return new object[] { new DateTime(2024, 1, 1), new DateTime(2024, 1, 1), 5 };
        yield return new object[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 7), 6 };
        yield return new object[] { new DateTime(2024, 1, 7), new DateTime(2023, 1, 8), 6 };
    }

    [Theory]
    [MemberData(nameof(QueryData))]
    public async Task Given_FromAndToParameters_Should_ReturnRoomsAvailableInThatPeriod(DateTime from, DateTime to, int expected)
    {
        var hotel = Hotel.Create("Holiday Bin");
        await _uow.Hotels.AddAsync(hotel);

        var booking = Booking.Create(hotel.Id, hotel.Rooms.First().Id, new DateTime(2024, 1, 1), new DateTime(2024, 1, 7));
        await _uow.Bookings.AddAsync(booking);
        hotel.Rooms.First().Bookings.Add(booking);

        var qry = new GetRoomsQuery(hotel.Id, from, to);
        var response = await _sut.Handle(qry, CancellationToken.None);

        var actual = response.Count();

        Assert.Equivalent(expected, actual);
    }
}
