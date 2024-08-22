namespace TendersApi.Domain.Entities;

public sealed class Supplier : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal Value { get; set; }
    public decimal ValueEur { get; set; }
    public TenderSupplier[] TenderSupplier { get; set; } = [];
}
