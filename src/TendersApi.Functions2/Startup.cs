using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TendersApi.Application;
using TendersApi.Infrastructure;

[assembly: FunctionsStartup(typeof(TendersApi.Functions.Startup))]

namespace TendersApi.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var databaseConnectionString = config.GetConnectionString("TendersDatabase")!;

        builder.Services
            .AddInfrastructure(databaseConnectionString)
            .AddApplication();

        if (builder.GetContext().EnvironmentName == "Development")
        {
        }
    }
}
