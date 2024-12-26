using Common.Domain;
using Common.Infrastructure.Configuration;
using Common.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Utilities.Infrastructure;

public class UtilitiesModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
            .AddScoped<IUnitOfWork, HotelAdmin.Persistence.UnitOfWork>()
            .AddTransientAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(Application.AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(Endpoints.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Application.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Endpoints.AssemblyReference.Assembly);
}