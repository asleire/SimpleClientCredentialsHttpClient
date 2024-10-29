using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SimpleClientCredentialsHttpClient;

internal class SimpleTokenHandler
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IClock _clock;
    private readonly SimpleOptions _options;

    private readonly SemaphoreSlim _tokenLock = new(1);
    private DateTime _nextRenewal;
    private Task<string>? _nextToken;

    public SimpleTokenHandler(IHttpClientFactory httpClientFactory, SimpleOptions options, IClock clock)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
        _clock = clock;
    }

    public async ValueTask<string> GetToken()
    {
        await _tokenLock.WaitAsync();

        try
        {
            if (_clock.UtcNow > _nextRenewal || _nextToken == null)
                _nextToken = RenewToken();

            return await _nextToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private async Task<string> RenewToken()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            var httpResponse = await httpClient.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = _options.TokenUrl,
                Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _options.ClientId,
                    ["client_secret"] = _options.ClientSecret,
                    ["scope"] = _options.Scope,
                }),
            });

            if (!httpResponse.IsSuccessStatusCode)
                await ThrowHttpException(httpResponse);

            var tokenResponse = await httpResponse.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null)
                throw new NullTokenResponseException();

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                throw new EmptyTokenResponseAccessTokenException();

            if (!tokenResponse.ExpiresIn.HasValue)
                throw new EmptyTokenResponseExpiresInException();

            _nextRenewal = _clock.UtcNow
                           + TimeSpan.FromSeconds(tokenResponse.ExpiresIn.Value)
                           - TimeSpan.FromSeconds(60);

            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _nextRenewal = _clock.UtcNow + TimeSpan.FromSeconds(5);
            throw new TokenErrorException(
                "Unexpected token renewal exception caught. Will not retry token retrieval for 5 seconds.", ex);
        }
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")] public string? AccessToken { get; set; }

        [JsonPropertyName("expires_in")] public long? ExpiresIn { get; set; }
    }

    [DoesNotReturn]
    private async Task ThrowHttpException(HttpResponseMessage message)
    {
        string? responseBody = null;
        Exception? responseBodyReadException = null;

        try
        {
            responseBody = await message.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            responseBodyReadException = ex;
        }

        throw new NonOkTokenResponseException((int)message.StatusCode, responseBody, responseBodyReadException);
    }
}
