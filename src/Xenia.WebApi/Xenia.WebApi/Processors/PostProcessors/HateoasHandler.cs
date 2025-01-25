using System.Collections.Concurrent;
using System.Linq.Expressions;

using Common.Endpoints.Hateoas;

using ErrorOr;

using FastEndpoints;

namespace Xenia.WebApi.Processors.PostProcessors;

// ReSharper disable once ClassNeverInstantiated.Global
public class HateoasHandler(IServiceScopeFactory scopeFactory) : IGlobalPostProcessor
{   
    public async Task PostProcessAsync(IPostProcessorContext ctx, CancellationToken ct)
    {
        if (ctx.HttpContext.ResponseStarted() || ctx.Response is not IErrorOr errorOr) return;

        if (errorOr.IsError)
            throw new InvalidOperationException($"Post-processing error: ${nameof(ErrorOrHandler)} should be called before ${nameof(HateoasHandler)}");

        var dto = GetValueFromErrorOr(errorOr);

        var enrichedResponse = EnrichResponse(dto, ctx.HttpContext);
        await ctx.HttpContext.Response.SendAsync(enrichedResponse, cancellation: ct);
    }

    private object EnrichResponse(object dto, HttpContext httpContext)
    {
        using var scope = scopeFactory.CreateScope();
        var linkGenerator = scope.ServiceProvider.GetRequiredService<LinkGenerator>();
        var enricher = GetEnricherForType(dto.GetType());
        return enricher != null ? enricher.Enrich(dto, linkGenerator, httpContext) :
            dto;
    }

    private IHateoasEnricher? GetEnricherForType(Type dtoType)
    {
        using var scope = scopeFactory.CreateScope();
        var type = typeof(IHateoasEnricher<>).MakeGenericType(dtoType);
        return scope.ServiceProvider.GetService(type) as IHateoasEnricher;
    }

    //cached compiled expressions for reading ErrorOr<T>.Value property
    static readonly ConcurrentDictionary<Type, Func<object, object>> _valueAccessors = new();

    static object GetValueFromErrorOr(object errorOr)
    {
        ArgumentNullException.ThrowIfNull(errorOr);
        var tErrorOr = errorOr.GetType();

        if (!tErrorOr.IsGenericType || tErrorOr.GetGenericTypeDefinition() != typeof(ErrorOr<>))
            throw new InvalidOperationException("The provided object is not an instance of ErrorOr<>.");

        return _valueAccessors.GetOrAdd(tErrorOr, CreateValueAccessor)(errorOr);

        static Func<object, object> CreateValueAccessor(Type errorOrType)
        {
            var parameter = Expression.Parameter(typeof(object), "errorOr");

            return Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(parameter, errorOrType),
                            "Value"),
                        typeof(object)),
                    parameter)
                .Compile();
        }
    }
}