using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public sealed class OrderByQueryableSubservice(IEnumerable<IOrderByPropertyTypeService> propertySorters) : IQueryableSubservice
{
    public bool CanHandle(SearchModelRequest searchModel)
        => searchModel is not null && searchModel.OrderBy is not null;

    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModelRequest searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel.OrderBy);

        if (!searchModel.OrderBy.Any())
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T));
        var orderByCriterias = searchModel.OrderBy.OrderBy(x => x.Index).ToArray();
        var firstCriteria = orderByCriterias[0];

        var sortedQuery = FirstSort(query, orderByCriterias[0], parameter);

        foreach (var orderByCriteria in orderByCriterias.Skip(1))
        {
            var propertyExpression = Expression.PropertyOrField(parameter, orderByCriteria.Field);
            var propertyType = propertyExpression.Type;

            var sorter = propertySorters.Single(x => x.CanSort(propertyType));
            sortedQuery = sorter.OrderByAscending(sortedQuery, propertyExpression, parameter);
        }

        return sortedQuery;
    }

    private IOrderedQueryable<T> FirstSort<T>(IQueryable<T> query, OrderByCriteria orderByCriteria, ParameterExpression parameter)
    {
        var propertyExpression = Expression.PropertyOrField(parameter, orderByCriteria.Field);
        var propertyType = propertyExpression.Type;
        var sorter = propertySorters.Single(x => x.CanSort(propertyType));

        return orderByCriteria.Direction == OrderDirection.Descending.ToString()
            ? sorter.OrderByDescending(query, propertyExpression, parameter)
            : sorter.OrderByAscending(query, propertyExpression, parameter);
    }
}
