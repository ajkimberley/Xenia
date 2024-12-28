using Common.Infrastructure.Extensions;

using ErrorOr;

using FastEndpoints;
using FastEndpoints.Swagger;

using Modules.Bookings.Persistence;

using Xenia.WebApi.Processors.PostProcessors;
using Xenia.WebApi.Processors.PostProcessors.HateoasEnrichment;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .InstallServicesFromAssemblies(
        builder.Configuration,
        Xenia.WebApi.AssemblyReference.Assembly,
        Common.Persistence.AssemblyReference.Assembly)
    .InstallModulesFromAssemblies(
        builder.Configuration,
        Modules.Bookings.Infrastructure.AssemblyReference.Assembly)
    .SwaggerDocument()
    .AddFastEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    using var hotelContext = scope.ServiceProvider.GetService<BookingContext>();
    _ = hotelContext?.Database.EnsureDeleted();
    _ = hotelContext?.Database.EnsureCreated();
}

app
    .UseFastEndpoints(
        c =>
        {
            c.Errors.UseProblemDetails();
            c.Endpoints.Configurator =
                ep =>
                {
                    if (!ep.ResDtoType.IsAssignableTo(typeof(IErrorOr))) return;
                    ep.DontAutoSendResponse();
                    ep.PostProcessor<ErrorOrHandler>(Order.After);
                    ep.PostProcessor<HateoasHandler>(Order.After);
                    ep.Description(
                        b => b.ClearDefaultProduces()
                            .Produces(200, ep.ResDtoType.GetGenericArguments()[0])
                            .ProducesProblemDetails());
                };
            c.Versioning.Prefix = "v";
            c.Versioning.DefaultVersion = 0;
        })
    .UseSwaggerGen();

app.Run();

// Partial Program class added to support integration testing
namespace Xenia.WebApi
{
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class Program;
}