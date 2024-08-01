using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public interface IOrderDirectionQueryableService
{
    bool CanHandle(OrderByCriteria orderByCriteria);
    IOrderedQueryable<T> OrderBy<T>(IQueryable<T> query, OrderByCriteria orderByCriteria);
    IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> query, OrderByCriteria orderByCriteria);
}
