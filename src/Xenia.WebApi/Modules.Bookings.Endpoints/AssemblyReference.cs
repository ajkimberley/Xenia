using System.Reflection;

namespace Modules.Bookings.Endpoints;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}