namespace TendersApi.Models;

public sealed class FilterCriteria
{
    public string Field { get; set; } = default!;
    public string? Value { get; set; }
    public string? Operator { get; set; }
}

public enum FilterableField
{
    SupplierId,
    PriceEur
}
