namespace TendersApi.Models;

public sealed class Tender
{
    public string TenderId { get; set; } = default!;
    public DateTime TenderDate { get; set; }
}
