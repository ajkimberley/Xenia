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
    public async Task Given_NoHotelsInDb_ShouldThrowException()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(Guid.NewGuid(), Guid.NewGuid(), "Joe", "Bloggs", from, to);

        _ = await Assert.ThrowsAsync<Exception>(() => _sut.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Given_ValidCommand_ShouldBookRoom()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 7);
        var cmd = new BookRoomCommand(Guid.NewGuid(), Guid.NewGuid(), "Joe", "Bloggs", from, to);

        var response = await _sut.Handle(cmd, CancellationToken.None);

        Assert.NotNull(response);
    }
}
