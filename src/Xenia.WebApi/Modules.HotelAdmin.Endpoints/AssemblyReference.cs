using System.Reflection;

namespace Modules.HotelAdmin.Endpoints;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}