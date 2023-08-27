using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;

namespace ScreenMedia.Xenia.WebApi.UnitTests;

public class CreateHotelCommandTests
{
    private readonly CreateHotelHandler _sut;
    private readonly FakeUow _uow;

    public CreateHotelCommandTests()
    {
        _uow = new FakeUow();
        _sut = new CreateHotelHandler(_uow);
    }

    [Theory]
    [InlineData("Travel Bodge")]
    [InlineData("Mediocre Inn")]
    [InlineData("Holiday Bin")]
    public async Task Given_ValidCommand_Should_ReturnId(string hotelName)
    {
        var cmd = new CreateHotelCommand(hotelName);

        await _sut.Handle(cmd, CancellationToken.None);

        Assert.NotEmpty(_uow.Hotels.GetAllAsync().Result);
    }
}