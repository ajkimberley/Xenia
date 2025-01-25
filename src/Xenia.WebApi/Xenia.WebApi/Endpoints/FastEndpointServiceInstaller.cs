using Common.Infrastructure.Configuration;

using FastEndpoints;

namespace Xenia.WebApi.Endpoints;

public class FastEndpointServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) => 
        services.AddFastEndpoints(options =>
        {
            options.DisableAutoDiscovery = true;
            options.Assemblies = EndpointRegistrationFactory.Instance.Assemblies;
        });
}