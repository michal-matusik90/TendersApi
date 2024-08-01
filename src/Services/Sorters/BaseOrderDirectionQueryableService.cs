using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Sorters;

public abstract class BaseOrderDirectionQueryableService
{
    protected Expression<Func<T, string>> CreateExpression<T>(OrderByCriteria orderByCriteria)
    {
        var parameter = Expression.Parameter(typeof(T));
        var propertyExpression = Expression.Property(parameter, orderByCriteria.Field);
        return Expression.Lambda<Func<T, string>>(propertyExpression, parameter);
    }
}