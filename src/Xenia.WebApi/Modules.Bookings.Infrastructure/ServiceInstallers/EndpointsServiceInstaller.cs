﻿using Common.Infrastructure.Configuration;
using Common.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Bookings.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module endpoints service installer.
/// </summary>
public class EndpointsServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddEndpointRegistrationsFromAssembly(Endpoints.AssemblyReference.Assembly);
}