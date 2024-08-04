namespace TendersApi.Models;

public sealed class SearchModelRequest
{
    public IEnumerable<FilterCriteria>? Filters { get; init; }
    public string? FilterCriteriaOperator { get; init; }
    public IEnumerable<OrderByCriteria>? OrderBy { get; init; }
    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 100;
}
