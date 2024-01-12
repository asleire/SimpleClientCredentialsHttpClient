using System.Net.Http.Headers;

namespace SimpleClientCredentialsHttpClient;

internal class SimpleDelegatingHandler : DelegatingHandler
{
    private readonly SimpleTokenHandler _tokenHandler;

    public SimpleDelegatingHandler(SimpleTokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    private async Task Process(HttpRequestMessage request)
    {
        if (request.Headers.Authorization != null)
            throw new AuthorizationHeaderAlreadySetException();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenHandler.GetToken());
    }
    
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Process(request).GetAwaiter().GetResult();
        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await Process(request);
        return await base.SendAsync(request, cancellationToken);
    }
}
