namespace TendersApi.Application.DTOs;

public sealed class TenderDto
{
    public string? Id { get; set; }
    public string? Date { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal ValueEur { get; set; }
    public IEnumerable<SupplierDto> Suppliers { get; set; } = [];
}
