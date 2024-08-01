using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using TendersApi.Data;
using TendersApi.Models;

var host = new HostBuilder()
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
    })
    .ConfigureFunctionsWebApplication((IFunctionsWorkerApplicationBuilder builder) =>
    {
        builder.Services.Configure<WorkerOptions>(workerOptions =>
        {
            var settings = NewtonsoftJsonObjectSerializer.CreateJsonSerializerSettings();

            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            workerOptions.Serializer = new NewtonsoftJsonObjectSerializer(settings);
        });
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient();
        services.AddOptions();

        services
            .AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TenderDb"));

        // Seed the database with sample data
        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Tenders.AddRange(
                new Tender { TenderId = "1", TenderDate = DateTime.UtcNow },
                new Tender { TenderId = "2", TenderDate = DateTime.UtcNow.AddDays(-1) },
                new Tender { TenderId = "3", TenderDate = DateTime.UtcNow.AddDays(-3) }
            );
            dbContext.SaveChanges();
        }
    })
    .Build();

await host.RunAsync();
