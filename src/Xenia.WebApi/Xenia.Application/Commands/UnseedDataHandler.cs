using MediatR;

using Xenia.Bookings.Domain;

namespace Xenia.Application.Commands;

public record UnseedDataCommand() : IRequest;

public class UnseedDataHandler(IUnitOfWork uow) : IRequestHandler<UnseedDataCommand>
{
    public async Task Handle(UnseedDataCommand cmd, CancellationToken cancellation)
    {
        var hotels = await uow.Hotels.GetAllAsync();
        var bookings = await uow.Bookings.GetAllAsync();

        uow.Hotels.DeleteRange(hotels);
        uow.Bookings.DeleteRange(bookings);
        _ = await uow.CompleteAsync(cancellation);
    }
}
