using TendersApi.Models;
using TendersApi.Services.Filters.Logical;

namespace TendersApi.Services.Filters;

public sealed class FilterQueryableSubservice(IEnumerable<ILogicalFilterQueryableService> logicalFilters) : IQueryableSubservice
{
    public bool CanHandle(SearchModel searchModel)
        => searchModel.Filters is not null;

    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModel searchModel)
    {
        if (searchModel.Filters is null)
        {
            return query;
        }

        if (searchModel.Filters.Count() == 1)
        {
            return logicalFilters.First().Handle(query, searchModel.Filters);
        }

        var logicalFilter = logicalFilters.Single(x => x.CanHandle(searchModel.FilterCriteriaOperator!));
        return logicalFilter.Handle(query, searchModel.Filters);
    }
}

