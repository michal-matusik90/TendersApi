﻿namespace TendersApi.Domain.Entities;

public sealed class Tender : BaseEntity
{
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public decimal Value { get; set; }
    public decimal ValueEur { get; set; }
    public TenderSupplier[] TenderSuppliers { get; set; } = [];
}
