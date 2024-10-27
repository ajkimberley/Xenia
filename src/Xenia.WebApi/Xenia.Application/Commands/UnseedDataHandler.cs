using MediatR;

using Xenia.Bookings.Domain;

namespace Xenia.Application.Commands;

public record UnseedDataCommand() : IRequest;

public class UnseedDataHandler : IRequestHandler<UnseedDataCommand>
{
    private readonly IUnitOfWork _uow;
    public UnseedDataHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(UnseedDataCommand cmd, CancellationToken cancellation)
    {
        var hotels = await _uow.Hotels.GetAllAsync();
        var bookings = await _uow.Bookings.GetAllAsync();

        _uow.Hotels.DeleteRange(hotels);
        _uow.Bookings.DeleteRange(bookings);
        _ = await _uow.CompleteAsync(cancellation);
    }
}
