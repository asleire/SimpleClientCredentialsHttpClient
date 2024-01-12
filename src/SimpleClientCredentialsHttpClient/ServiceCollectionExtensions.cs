using Microsoft.Extensions.DependencyInjection;

namespace SimpleClientCredentialsHttpClient;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        SimpleOptions options)
    {
        return services.AddHttpClient(name).AddSimpleClientCredentialsAuthorization(options);
    }

    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        SimpleOptions options) where TClient : class
    {
        return services.AddHttpClient<TClient>().AddSimpleClientCredentialsAuthorization(options);
    }
    
    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        Func<IServiceProvider, SimpleOptions> optionsFn)
    {
        return services.AddHttpClient(name).AddSimpleClientCredentialsAuthorization(optionsFn);
    }

    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        Func<IServiceProvider, SimpleOptions> optionsFn) where TClient : class
    {
        return services.AddHttpClient<TClient>().AddSimpleClientCredentialsAuthorization(optionsFn);
    }
}
