namespace TendersApi.Models;

public sealed class ExternalResponse
{
    public int PageCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public Data[] Data { get; set; }
}

public sealed class Data
{
    public string Id { get; set; }
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string Sid { get; set; }
    public string AwardedValue { get; set; }
    public string AwardedCurrency { get; set; }
    public string AwardedValueEur { get; set; }
    public Awarded[] Awarded { get; set; }
}

public sealed class Awarded
{
    public Supplier[] Suppliers { get; set; }
    public string Value { get; set; }
    public decimal ValueEur { get; set; }
}

public sealed class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
}
