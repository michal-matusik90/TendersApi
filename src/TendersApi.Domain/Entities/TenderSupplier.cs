namespace TendersApi.Domain.Entities;

public class TenderSupplier : BaseEntity
{
    public string TenderId { get; set; } = default!;
    public virtual Tender Tender { get; set; } = default!;

    public string SupplierId { get; set; } = default!;
    public virtual Supplier Supplier { get; set; } = default!;
}
