using Common.Domain;

namespace Modules.HotelAdmin.Domain.Availabilities;

public class Availability : Entity
{
    public required Guid HotelId { get; init; }
    public required string RoomType { get; init; }
    public required DateTime Date { get; init; }
    public required int AvailableRooms { get; set; }
}