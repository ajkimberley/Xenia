using Xenia.Application.Availabilities;
using Xenia.WebApi.Commands.UnitTests.Fakes;

namespace Xenia.WebApi.Commands.UnitTests;

public class SetAvailabilityTests
{
    public class UpsertAvailabilityCommandHandlerTests
    {
        private readonly SetAvailabilityHandler _sut;
        private readonly FakeUow _uow;

        public UpsertAvailabilityCommandHandlerTests()
        {
            _uow = new FakeUow();
            _sut = new SetAvailabilityHandler(_uow, _uow.Availability);
        }

        [Fact]
        public async Task GivenANewDate_ShouldAddAvailabilityForThatDate()
        {
            const int expectedCount = 10;
            const string expectedRoomType = "Single";
            var expectedHotelId = Guid.NewGuid();
            var expectedAvailabilityDate = new DateTime(2024, 1, 1);

            var command = new SetAvailabilityCommand(expectedHotelId, expectedRoomType, expectedAvailabilityDate, expectedCount);
            await _sut.Handle(command, CancellationToken.None);

            var availabilities = await _uow.Availability.GetAllAsync();
            var availabilitiesList = availabilities.ToList();

            Assert.Single(availabilitiesList);
            var availability = availabilitiesList.First();

            Assert.Multiple(() =>
            {
                Assert.Equal(expectedRoomType, availability.RoomType, StringComparer.InvariantCultureIgnoreCase);
                Assert.Equal(expectedAvailabilityDate, availability.Date);
                Assert.Equal(expectedHotelId, availability.HotelId);
                Assert.Equal(expectedCount, availability.AvailableRooms);
            });
        }
    }
}