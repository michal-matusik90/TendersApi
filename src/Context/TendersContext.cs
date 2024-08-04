using Microsoft.EntityFrameworkCore;
using TendersApi.Context.Models;

namespace TendersApi.Context;

public class TendersContext : DbContext
{
    public virtual DbSet<Tender> Tenders { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }

    public TendersContext(DbContextOptions options) : base(options)
    {
    }

    protected TendersContext()
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
