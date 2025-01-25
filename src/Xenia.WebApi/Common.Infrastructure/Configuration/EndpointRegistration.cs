using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Configuration;

public class EndpointRegistrationFactory
{
    private static readonly Lazy<EndpointRegistrationFactory> _instance = new(() => new EndpointRegistrationFactory());
    
    public IList<Assembly> Assemblies { get; private set;  } = new List<Assembly>();
    
    public static EndpointRegistrationFactory Instance => _instance.Value;

    public void RegisterAssemblyEndpoints(Assembly assembly) => Assemblies.Add(assembly);
}