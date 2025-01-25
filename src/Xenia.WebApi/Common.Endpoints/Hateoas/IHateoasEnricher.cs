using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Common.Endpoints.Hateoas;

public interface IHateoasEnricher
{
    object Enrich(object dto, LinkGenerator urlHelper, HttpContext httpContext);
}

public interface IHateoasEnricher<in T> : IHateoasEnricher;
