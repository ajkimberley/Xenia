namespace Xenia.WebApi.RequestResponse;

public record AvailabilitiesRequest(Guid HotelId, DateTime Date, string RoomType, int AvailableRooms);

public record AvailabilitiesResponse();