using Microsoft.AspNetCore.Mvc.Testing;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;
public class XeniaWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => _ = builder.UseEnvironment("Development");
}
