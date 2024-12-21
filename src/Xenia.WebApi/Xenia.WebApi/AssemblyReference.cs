using System.Reflection;

namespace Xenia.WebApi;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}