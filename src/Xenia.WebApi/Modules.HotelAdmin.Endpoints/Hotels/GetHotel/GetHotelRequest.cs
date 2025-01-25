using FastEndpoints;

namespace Modules.HotelAdmin.Endpoints.Hotels.GetHotel;

public sealed class GetHotelRequest
{
    [BindFrom("id")] 
    public Guid Id { get; set; }
}