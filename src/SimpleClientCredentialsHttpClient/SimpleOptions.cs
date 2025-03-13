using Microsoft.Extensions.Options;

namespace SimpleClientCredentialsHttpClient;

public record SimpleOptions
{
    /// <summary>
    /// Complete URL of the OAuth server token endpoint, e.g. https://login.mycompany.com/connect/token
    /// </summary>
    public required Uri TokenUrl { get; init; }
    
    /// <summary>
    /// Client ID
    /// </summary>
    public required string ClientId { get; set; }
    
    /// <summary>
    /// Client Secret
    /// </summary>
    public required string ClientSecret { get; set; }
    
    /// <summary>
    /// Scope
    /// </summary>
    public required string Scope { get; set; }

    /// <summary>
    /// Token requests will fail and be retried if they take longer than this.
    /// </summary>
    public TimeSpan TokenTimeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The name of the HttpClient to use when requesting a token. Defaults to the standard HttpClient.  
    /// </summary>
    public string HttpClientName { get; set; } = Options.DefaultName;
}
