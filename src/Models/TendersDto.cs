namespace TendersApi.Models;

public sealed class TendersDto
{
    public int Skip { get; init; }
    public int Take { get; init; }
    public int Total { get; init; }
    public IEnumerable<TenderDto> Tenders { get; init; } = [];
}
