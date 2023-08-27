using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.HotelManagement.Persistence;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;
public class XeniaWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<HotelManagementContext>));
            _ = services.Remove(dbContextDescriptor);
            _ = services.AddDbContext<HotelManagementContext>((container, options)
                => options.UseSqlServer("Server=localhost;Database=XeniaTest;Trusted_Connection=True;TrustServerCertificate=True"));

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<HotelManagementContext>();
            _ = context.Database.EnsureDeleted();
            _ = context.Database.EnsureCreated();
        });
        _ = builder.UseEnvironment("Development");
    }
}
