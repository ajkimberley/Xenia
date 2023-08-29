using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Bookings.Persistence;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

[CollectionDefinition("WebApi Collection")]
public class WebApiCollection : ICollectionFixture<XeniaWebApplicationFactory<Program>>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class XeniaWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<BookingContext>));
            _ = services.Remove(dbContextDescriptor);
            _ = services.AddDbContext<BookingContext>((container, options)
                => options.UseSqlServer("Server=localhost;Database=XeniaTest;Trusted_Connection=True;TrustServerCertificate=True"));

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<BookingContext>();
            _ = context.Database.EnsureDeleted();
            _ = context.Database.EnsureCreated();
        });
        _ = builder.UseEnvironment("Development");
    }
}
