using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TendersApi.Domain.Entities;

namespace TendersApi.Infrastructure.Data.Configurations;

internal sealed class TenderConfiguration : BaseEntityConfiguration<Tender>
{
    public override void Configure(EntityTypeBuilder<Tender> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.TenderSuppliers)
            .WithOne(x => x.Tender)
            .HasForeignKey(x => x.TenderId);

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.Title)
            .HasMaxLength(200);

        builder.Property(x => x.ValueEur)
            .HasPrecision(18, 4);

        builder.Property(x => x.Value)
            .HasPrecision(18, 4);
    }
}
