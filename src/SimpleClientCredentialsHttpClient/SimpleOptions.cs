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
}
