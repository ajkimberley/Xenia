using Common.Domain;

using ErrorOr;

using MediatR;

using Modules.HotelAdmin.Domain.Availabilities;

namespace Modules.HotelAdmin.Application.SetAvailability;

public record SetAvailabilityCommand(Guid HotelId, string RoomType, DateTime Date, int Count) : IRequest<ErrorOr<Success>>;

public class SetAvailabilityHandler(IUnitOfWork unitOfWork, IAvailabilityRepository availabilityRepo) : IRequestHandler<SetAvailabilityCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(SetAvailabilityCommand request, CancellationToken cancellationToken)
    {
        await availabilityRepo.AddAsync(new Availability { HotelId = request.HotelId, RoomType = request.RoomType, Date = request.Date, AvailableRooms = request.Count } );
        return Result.Success;
    }
}