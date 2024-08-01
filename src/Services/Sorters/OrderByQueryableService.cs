using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public sealed class OrderByQueryableSubservice(IEnumerable<IOrderDirectionQueryableService> sorters) : IQueryableSubservice
{
    public bool CanHandle(SearchModel searchModel)
        => searchModel is not null && searchModel.OrderBy is not null;

    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel.OrderBy);

        var sortCriterias = searchModel.OrderBy.OrderBy(x => x.Index).ToArray();

        var firstCriteria = sortCriterias[0];
        var orderedQuery = GetSorter(firstCriteria).OrderBy(query, firstCriteria);

        for (int i = 1; i < sortCriterias.Length; i++)
        {
            var sorter = GetSorter(sortCriterias[i]);
            orderedQuery = sorter.ThenBy(orderedQuery, sortCriterias[i]);
        }

        return orderedQuery;
    }

    private IOrderDirectionQueryableService GetSorter(OrderByCriteria criteria)
        => sorters.Single(sorter => sorter.CanHandle(criteria));
}
