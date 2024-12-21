using System.Reflection;

namespace Modules.HotelAdmin.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}