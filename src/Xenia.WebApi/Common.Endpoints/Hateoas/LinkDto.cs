using System.Diagnostics.CodeAnalysis;

namespace Common.Endpoints.Hateoas;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record LinkDto(string Href, string Rel, string Method);
