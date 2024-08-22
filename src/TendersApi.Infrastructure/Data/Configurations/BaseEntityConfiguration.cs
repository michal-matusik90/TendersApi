using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TendersApi.Domain.Common;

namespace TendersApi.Infrastructure.Data.Configurations;

internal abstract class BaseEntityConfiguration<T>
    : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
