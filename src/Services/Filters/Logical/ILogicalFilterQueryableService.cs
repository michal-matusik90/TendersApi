using TendersApi.Models;

namespace TendersApi.Services.Filters.Logical;

public interface ILogicalFilterQueryableService
{
    public bool CanHandle(string logicalOperator);
    public IQueryable<T> Handle<T>(IQueryable<T> query, IEnumerable<FilterCriteria> filterCriterias);
}

