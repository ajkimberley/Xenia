using System.Reflection;

namespace Modules.Bookings.IntegrationEvents;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}