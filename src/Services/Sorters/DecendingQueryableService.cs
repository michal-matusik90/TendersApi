using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public sealed class DecendingQueryableService : BaseOrderDirectionQueryableService, IOrderDirectionQueryableService
{
    public bool CanHandle(OrderByCriteria orderByCriteria)
        => string.Equals(orderByCriteria.Direction, OrderDirection.Descending.ToString(), StringComparison.OrdinalIgnoreCase);

    public IOrderedQueryable<T> OrderBy<T>(IQueryable<T> query, OrderByCriteria orderByCriteria)
    {
        var lambda = CreateExpression<T>(orderByCriteria);
        return query.OrderByDescending(lambda);
    }

    public IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> query, OrderByCriteria orderByCriteria)
    {
        var lambda = CreateExpression<T>(orderByCriteria);
        return query.ThenByDescending(lambda);
    }
}