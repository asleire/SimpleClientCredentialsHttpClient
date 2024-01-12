using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleClientCredentialsHttpClient.Test;

public class Tests
{
    private SimpleOptions _options = null!;

    [SetUp]
    public void Setup()
    {
        var options = File.ReadAllText("test_credentials.secret");

        if (string.IsNullOrEmpty(options))
            throw new Exception("Options not set");

        _options = JsonSerializer.Deserialize<SimpleOptions>(options) ?? throw new
            InvalidOperationException();
    }

    [Test]
    public async Task Test()
    {
        var sc = new ServiceCollection();
        var clock = new MockClock();
        var handler = new TestHandler();
        sc.AddSingleton<IClock>(clock);

        sc.AddHttpClient("test")
            .AddClientCredentialsAuthorization(_options)
            .AddHttpMessageHandler(() => handler);
        
        sc.AddSimpleClientCredentialsHttpClient("test", _options);

        await using var sp = sc.BuildServiceProvider();

        var httpClient = sp
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("test");

        await httpClient.GetAsync("https://localhost");
        var token1 = handler.LastToken;
        clock.UtcNow += TimeSpan.FromSeconds(10);
        
        await httpClient.GetAsync("https://localhost");
        var token2 = handler.LastToken;
        clock.UtcNow += TimeSpan.FromDays(1);
        
        await httpClient.GetAsync("https://localhost");
        var token3 = handler.LastToken;

        Assert.That(token2, Is.EqualTo(token1));
        Assert.That(token3, Is.Not.EqualTo(token2));
    }
    
    private class TestHandler : DelegatingHandler
    {
        public string? LastToken { get; set; }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastToken = request.Headers.Authorization?.Parameter ?? throw new Exception("No auth header value");
            return Task.FromResult(new HttpResponseMessage());
        }
    }

    private class MockClock : IClock
    {
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
