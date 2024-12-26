using Common.Domain;
using Common.Infrastructure.Configuration;
using Common.Persistence.Extensions;
using Common.Persistence.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Modules.HotelAdmin.Domain.Availabilities;
using Modules.HotelAdmin.Domain.Hotels;
using Modules.HotelAdmin.Persistence;
using Modules.HotelAdmin.Persistence.Constants;
using Modules.HotelAdmin.Persistence.Repositories;

namespace Modules.HotelAdmin.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module persistence service installer.
/// </summary>
public sealed class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<HotelAdminContext>((serviceProvider, options) =>
            {
                var connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;
                options.UseSqlServer(
                    connectionString,
                    dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.HotelAdmin));
            })
            .AddScoped<IHotelRepository, HotelRepository>()
            .AddScoped<IAvailabilityRepository, AvailabilityRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
}