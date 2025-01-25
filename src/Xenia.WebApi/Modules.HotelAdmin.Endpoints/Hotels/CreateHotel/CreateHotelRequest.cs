using FastEndpoints;

namespace Modules.HotelAdmin.Endpoints.Hotels.CreateHotel;

public class CreateHotelRequest
{
    [BindFrom("name")]
    public string Name { get; set; }
}