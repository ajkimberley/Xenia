using System.Reflection;

namespace Modules.Utilities.Endpoints;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}