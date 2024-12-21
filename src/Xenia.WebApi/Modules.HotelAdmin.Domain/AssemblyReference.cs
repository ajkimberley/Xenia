using System.Reflection;

namespace Modules.HotelAdmin.Domain;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}