using Common.Infrastructure.Configuration;
using Common.Persistence.Extensions;
using Common.Persistence.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Modules.Bookings.Persistence;
using Modules.Bookings.Persistence.Constants;

namespace Modules.Bookings.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module persistance service installer.
/// </summary>
public sealed class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<BookingContext>((serviceProvider, options) =>
            {
                var connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;
                options.UseSqlServer(
                    connectionString,
                    dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.Booking));
            });
}