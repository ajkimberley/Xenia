using FastEndpoints;

namespace Modules.HotelAdmin.Endpoints.Hotels.GetHotels;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetHotelsRequest
{
    [BindFrom("name")]
    public string? Name { get; set; }
}