public class AuthenticationTests
{
    using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                })
                .Configure(app =>
                {
                    app.UseMiddleware<Authentication>();
                });
    })
    .StartAsync();

    [Fact]
    public async Task MiddlewareTest_FailNotAuthorized()
    {
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Failed!", result);
    }

    [Fact]
    public async Task MiddlewareTest_Authenticated()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authenticated!", result);
    }

    [Fact]
    public async Task MiddlewareTest_FailJustUserName()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user2");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Fail!", result);
    }

    [Fact]
    public async Task MiddlewareTest_FailDifferentUserAndPassord()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user2&password=password2");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Fail!", result);
    }
}