using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TendersApi.Context;

namespace TendersApi.Migrations;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<TendersContext>
{
    public TendersContext CreateDbContext(string[] args)
    {
        var databaseConnectionString = GetDatabaseConnectionString("TendersDatabase");

        var optionsBuilder = new DbContextOptionsBuilder<TendersContext>()
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .UseSqlServer(
                databaseConnectionString,
                x => x.MigrationsAssembly(typeof(DatabaseContextFactory).Assembly.FullName));

        return new TendersContext(optionsBuilder.Options);
    }

    private static string GetDatabaseConnectionString(string name)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var databaseConnectionString = configuration.GetConnectionString(name);

        return databaseConnectionString
            ?? throw new ArgumentNullException($"There is no connection string under name: {name}");
    }
}
