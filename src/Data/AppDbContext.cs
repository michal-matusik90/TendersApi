using Microsoft.EntityFrameworkCore;
using TendersApi.Models;

namespace TendersApi.Data;

public class AppDbContext : DbContext
{
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Tender> Tenders { get; set; }
}
