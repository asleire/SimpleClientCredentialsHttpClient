using Microsoft.Extensions.DependencyInjection;
using SimpleClientCredentialsHttpClient;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddSimpleClientCredentialsAuthorization(this IHttpClientBuilder builder,
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
    
    public static IHttpClientBuilder AddSimpleClientCredentialsAuthorization(this IHttpClientBuilder builder,
        SimpleOptions options)
    {
        return builder.AddSimpleClientCredentialsAuthorization(_ => options);
    }
}
