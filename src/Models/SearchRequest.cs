namespace TendersApi.Models;

public sealed class SearchModel
{
    public IEnumerable<FilterCriteria>? Filters { get; set; }
    public string? FilterCriteriaOperator { get; set; }
    public IEnumerable<OrderByCriteria>? OrderBy { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 100;
}
