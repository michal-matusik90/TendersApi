namespace TendersApi.Infrastructure.Data;
/*
public sealed class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public void SeedAsync()
    {
        try
        {
            TrySeed();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public void TrySeed()
    {
        var tasks = Enumerable.Range(1, 100)
            .Select(page => tendersApiService.GetTenders(page, default))
            .ToList();

        context.SaveChanges();
    }
}

*/