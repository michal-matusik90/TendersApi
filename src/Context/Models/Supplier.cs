namespace TendersApi.Context.Models;

public sealed class Supplier : BaseEntity
{
    public Tender Tender { get; set; }
    public string Name { get; set; } = default!;
    public decimal Value { get; set; }
    public decimal ValueEur { get; set; }
}
