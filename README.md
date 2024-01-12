# SimpleClientCredentialsHttpClient
[![NuGet version (SimpleClientCredentialsHttpClient)](https://img.shields.io/nuget/v/SimpleClientCredentialsHttpClient.svg?style=flat-square)](https://www.nuget.org/packages/SimpleClientCredentialsHttpClient/)

SimpleClientCredentialsHttpClient is a C# library designed for very easily using HttpClient with OAuth2 client credentials authentication.

## Dependencies
SimpleClientCredentialsHttpClient is designed for work with .NET Core apps, and make use of the following packages that are probably already a part of your application:
* `Microsoft.Extensions.DependencyInjection.Abstractions`
* `Microsoft.Extensions.Http`
* `System.Text.Json`

## Usage

### IServiceCollection

The simplest setup is done using `IServiceCollection`.

```csharp
// IServiceProvider services = ...; // Should be available somewhere in your Startup.cs or Program.cs
services
    .AddSimpleClientCredentialsHttpClient(nameof(MyApiClient), new SimpleOptions() { // This will apply an Authorization header to requests from HttpClient with name 'MyApiClient'
        TokenUrl = "https://login.my-company.com/connect/token", // TODO Fetch settings from config
        ClientId = "my-client-id",
        ClientSecret = "my-client-secret",
        Scope = "my-api-scope",
    })
    .ConfigureHttpClient(http => // Optionally apply further configuration to the HttpClient
    {
        http.BaseAddress = new Uri("https://api.my-company.com);
    });

// IHttpClientFactory httpClientFactory = ...; // Should be injected where you need to use the API Client
var apiClient = httpClientFactory.CreateClient(nameof(MyApiClient));

var apiResponse = await apiClient.GetAsync("/sensitive-stuff");
```

### Token handling

The acquired access token is cached in-memory and only renewed 1 minute before its expiration.

If any error occurs during access token retrieval an exception will be thrown. This exception is cached for 5 seconds, after which the next request will retry fetching an access token.
