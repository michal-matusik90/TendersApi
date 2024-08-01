using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public sealed class AscendingQueryableService : BaseOrderDirectionQueryableService, IOrderDirectionQueryableService
{
    public bool CanHandle(OrderByCriteria orderByCriteria)
        => string.Equals(orderByCriteria.Direction, OrderDirection.Ascending.ToString(), StringComparison.OrdinalIgnoreCase);

    public IOrderedQueryable<T> OrderBy<T>(IQueryable<T> query, OrderByCriteria orderByCriteria)
    {
        var lambda = CreateExpression<T>(orderByCriteria);
        return query.OrderBy(lambda);
    }

    public IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> query, OrderByCriteria orderByCriteria)
    {
        var lambda = CreateExpression<T>(orderByCriteria);
        return query.ThenBy(lambda);
    }
}
