using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TendersApi.Data;
using TendersApi.Models;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
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
