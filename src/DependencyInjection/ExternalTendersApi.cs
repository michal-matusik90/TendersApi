using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TendersApi.Models;
using TendersApi.Services;

namespace TendersApi.DependencyInjection;

internal static class ExternalTendersApi
{
    public static IServiceCollection AddExternalTendersApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ExternalTendersApiService>();

        var apiConfigSection = configuration.GetSection(nameof(ExternalTendersApiConfiguration));
        var apiConfig = new ExternalTendersApiConfiguration();
        apiConfigSection.Bind(apiConfig);

        services.Configure<ExternalTendersApiConfiguration>(apiConfigSection);

        services.AddHttpClient(nameof(ExternalTendersApiService), (serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<ExternalTendersApiConfiguration>>().Value;
            client.BaseAddress = new Uri(settings.BaseAddress);
        });

        return services;
    }
}
