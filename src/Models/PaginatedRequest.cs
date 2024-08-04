namespace TendersApi.Models;

public sealed class PaginatedRequest
{
    public string? Skip { get; init; }
    public string? Take { get; init; }
}