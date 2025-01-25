using Modules.HotelAdmin.Application;

namespace Modules.HotelAdmin.Endpoints.Hotels.GetHotels;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetHotelsResponse
{
    public IEnumerable<HotelDto> Hotels { get; set; }
}