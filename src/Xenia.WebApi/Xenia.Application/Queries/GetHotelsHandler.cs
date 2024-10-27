﻿using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;

namespace Xenia.Application.Queries;

public record GetHotelsQuery(string? Name = null) : IRequest<IEnumerable<HotelDto>>;

public class GetHotelsHandler : IRequestHandler<GetHotelsQuery, IEnumerable<HotelDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHotelsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels =
            request.Name != null
            ? await _unitOfWork.Hotels.GetAllAsync(request.Name)
            : await _unitOfWork.Hotels.GetAllAsync();
        var dtos = hotels.Select(hotel => new HotelDto(hotel.Name, hotel.Id));
        return dtos;
    }
}
