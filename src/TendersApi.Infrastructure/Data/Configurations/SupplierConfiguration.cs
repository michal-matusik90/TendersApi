using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TendersApi.Domain.Entities;

namespace TendersApi.Infrastructure.Data.Configurations;

internal sealed class SupplierConfiguration : BaseEntityConfiguration<Supplier>
{
    public override void Configure(EntityTypeBuilder<Supplier> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.ValueEur).HasPrecision(18, 4);
        builder.Property(x => x.Name).HasMaxLength(200);
    }
}
