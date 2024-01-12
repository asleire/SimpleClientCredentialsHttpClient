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
services.AddSimpleClientCredentialsHttpClient("my-api-client", new SimpleOptions() {
    TokenUrl = "https://login.my-company.com/connect/token", // TODO Fetch settings from config
    ClientId = "my-client-id",
    ClientSecret = "my-client-secret",
    Scope = "my-api-scope",
});

// IHttpClientFactory httpClientFactory = ...; // Should be injected where you need to use the API Client
var apiClient = httpClientFactory.CreateClient("my-api-client");

var apiResponse = await apiClient.GetAsync("https://api.my-company.com/sensitive-stuff");
```