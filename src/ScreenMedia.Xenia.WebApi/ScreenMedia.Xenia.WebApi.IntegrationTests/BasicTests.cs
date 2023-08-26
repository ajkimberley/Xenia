namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

public sealed class BasicTests : IClassFixture<XeniaWebApplicationFactory<Program>>
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public BasicTests(XeniaWebApplicationFactory<Program> applicationFactory) => _applicationFactory = applicationFactory;

    [Fact]
    public async void HelloHealthCheck()
    {
        var client = _applicationFactory.CreateClient();
        var expected = "Hello Xenia";

        var response = await client.GetAsync("api/Hello");
        var actual = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.Equal(expected, actual);
        });
    }
}