using Microsoft.Extensions.DependencyInjection;
using TendersApi.Infrastructure.Data;
using TendersApi.Infrastructure.ExternalApi.TendersApi.Services;

namespace TendersApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services
            .AddTransient<ITendersService, ExternalApiTendersService>()
            // .AddScoped<ApplicationDbContextInitialiser>()
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, contextOptions =>
                {
                    contextOptions.EnableRetryOnFailure();
                });
            })
            .AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

    }
}
