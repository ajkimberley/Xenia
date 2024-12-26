using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Common.Endpoints.Hateoas;

public abstract class HateoasEnricherBase<T> : IHateoasEnricher<T>
{
    public object Enrich(object dto, LinkGenerator linkGenerator, HttpContext httpContext)
    {
        if (dto is T typedDto)
        {
            return Enrich(typedDto, linkGenerator, httpContext);
        }

        throw new InvalidOperationException($"Invalid type: expected {typeof(T)}, got {dto.GetType()}");
    }

    public abstract object Enrich(T dto, LinkGenerator linkGenerator, HttpContext httpContext);
}