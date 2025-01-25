using FastEndpoints.Swagger;

namespace Xenia.WebApi.Swagger;

public static class SwaggerDocumentSetup
{
    public static void Configure(IServiceCollection services) =>
        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "Xenia API";
                s.Version = "v1";
                s.Description =
                    "The Xenia API is a RESTful service designed to help hotel administrators manage room availability and bookings efficiently. It features real-time availability syncing, booking simulations, search and filtering capabilities, and secure role-based access for admins.";
            };
        });
}