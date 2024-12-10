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
        public async Task Given_AvailabilityDoesNotExist_Should_CreateAvailability()
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
        
        [Fact]
        public async Task Given_AvailabilityExists_Should_Update()
        {
            const int originalCount = 10;
            const string expectedRoomType = "Single";
            var expectedHotelId = Guid.NewGuid();
            var expectedAvailabilityDate = new DateTime(2024, 1, 1);

            var command1 = new SetAvailabilityCommand(expectedHotelId, expectedRoomType, expectedAvailabilityDate, originalCount);
            await _sut.Handle(command1, CancellationToken.None);

            const int updatedCount = 20;
            var command2 = new SetAvailabilityCommand(expectedHotelId, expectedRoomType, expectedAvailabilityDate, updatedCount);
            await _sut.Handle(command2, CancellationToken.None);

            var availabilities = await _uow.Availability.GetAllAsync();
            var availabilitiesList = availabilities.ToList();

            Assert.Single(availabilitiesList);
            var availability = availabilitiesList.First();

            Assert.Multiple(() =>
            {
                Assert.Equal(expectedRoomType, availability.RoomType, StringComparer.InvariantCultureIgnoreCase);
                Assert.Equal(expectedAvailabilityDate, availability.Date);
                Assert.Equal(expectedHotelId, availability.HotelId);
                Assert.Equal(updatedCount, availability.AvailableRooms);
            });
        }
    }
}