using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;

namespace ScreenMedia.Xenia.HotelManagement.Domain;
public interface IUnitOfWork : IDisposable
{
    IHotelRepository Hotels { get; }
    Task<int> CompleteAsync();
}
