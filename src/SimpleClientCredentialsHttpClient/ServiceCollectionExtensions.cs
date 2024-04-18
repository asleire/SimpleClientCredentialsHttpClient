using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleClientCredentialsHttpClient;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        SimpleOptions options)
    {
        services.TryAddSingleton<SimpleTokenAccessor>();
        return services.AddHttpClient(name).AddSimpleClientCredentialsAuthorization(options);
    }

    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        SimpleOptions options) where TClient : class
    {
        services.TryAddSingleton<SimpleTokenAccessor>();
        return services.AddHttpClient<TClient>().AddSimpleClientCredentialsAuthorization(options);
    }
    
    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        Func<IServiceProvider, SimpleOptions> optionsFn)
    {
        services.TryAddSingleton<SimpleTokenAccessor>();
        return services.AddHttpClient(name).AddSimpleClientCredentialsAuthorization(optionsFn);
    }

    public static IHttpClientBuilder AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        Func<IServiceProvider, SimpleOptions> optionsFn) where TClient : class
    {
        services.TryAddSingleton<SimpleTokenAccessor>();
        return services.AddHttpClient<TClient>().AddSimpleClientCredentialsAuthorization(optionsFn);
    }
}
