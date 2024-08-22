using Microsoft.EntityFrameworkCore;
using TendersApi.Domain.Entities;

namespace TendersApi.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Supplier> Suppliers { get; }
    DbSet<Tender> Tenders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
