using System.Collections.Concurrent;
using System.Reflection;

namespace SimpleClientCredentialsHttpClient;

/// <summary>
/// Helper class for getting access tokens used by named HTTP clients
/// </summary>
public class SimpleTokenAccessor
{
    private readonly ConcurrentDictionary<string, SimpleTokenHandler> _handlers = new();
    private readonly IHttpClientFactory _httpClientFactory;

    public SimpleTokenAccessor(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async ValueTask<string> GetAccessToken(string httpClientName)
    {
        if (!_handlers.TryGetValue(httpClientName, out var handler))
        {

            var httpClient = _httpClientFactory.CreateClient(httpClientName);

            var httpHandler = typeof(HttpMessageInvoker)
                .GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(httpClient);

            while (httpHandler is DelegatingHandler delegatingHandler and not SimpleDelegatingHandler)
            {
                httpHandler = delegatingHandler.InnerHandler;
            }

            if (httpHandler is not SimpleDelegatingHandler simpleDelegatingHandler)
                throw new Exception("SimpleDelegatingHandler not found for this HttpClient");

            handler = _handlers[httpClientName] = simpleDelegatingHandler.TokenHandler;
        }

        return await handler.GetToken();
    }
}
