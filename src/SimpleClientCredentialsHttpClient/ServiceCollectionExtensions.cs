using Microsoft.Extensions.DependencyInjection;

namespace SimpleClientCredentialsHttpClient;

public static class ServiceCollectionExtensions
{
    public static void AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        SimpleOptions options)
    {
        services.AddHttpClient(name).AddClientCredentialsAuthorization(options);
    }

    public static void AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        SimpleOptions options) where TClient : class
    {
        services.AddHttpClient<TClient>().AddClientCredentialsAuthorization(options);
    }
    
    public static void AddSimpleClientCredentialsHttpClient(this IServiceCollection services, string name,
        Func<IServiceProvider, SimpleOptions> optionsFn)
    {
        services.AddHttpClient(name).AddClientCredentialsAuthorization(optionsFn);
    }

    public static void AddSimpleClientCredentialsHttpClient<TClient>(this IServiceCollection services,
        Func<IServiceProvider, SimpleOptions> optionsFn) where TClient : class
    {
        services.AddHttpClient<TClient>().AddClientCredentialsAuthorization(optionsFn);
    }
}

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddClientCredentialsAuthorization(this IHttpClientBuilder builder,
        Func<IServiceProvider, SimpleOptions> optionsFn)
    {
        return builder.AddHttpMessageHandler(sp =>
            {
                var options = optionsFn(sp);
                var clock = sp.GetService<IClock>() ?? new SimpleClock();

                return new SimpleDelegatingHandler(
                    ActivatorUtilities.CreateInstance<SimpleTokenHandler>(sp, options, clock)
                );
            }
        );
    }
    
    public static IHttpClientBuilder AddClientCredentialsAuthorization(this IHttpClientBuilder builder,
        SimpleOptions options)
    {
        return builder.AddClientCredentialsAuthorization(_ => options);
    }
}
