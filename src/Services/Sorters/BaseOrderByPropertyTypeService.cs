using System.Linq.Expressions;

namespace TendersApi.Services.Sorters;

public sealed class BaseOrderByPropertyTypeService<U> : IOrderByPropertyTypeService
{
    public bool CanSort(Type type)
        => type == typeof(U);

    public IOrderedQueryable<T> OrderByAscending<T>(IQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter)
        => query.OrderBy(CreateLambda<T>(propertyExpression, parameter));

    public IOrderedQueryable<T> OrderByAscending<T>(IOrderedQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter)
        => query.ThenBy(CreateLambda<T>(propertyExpression, parameter));

    public IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter)
        => query.OrderByDescending(CreateLambda<T>(propertyExpression, parameter));

    public IOrderedQueryable<T> OrderByDescending<T>(IOrderedQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter)
        => query.ThenByDescending(CreateLambda<T>(propertyExpression, parameter));

    private static Expression<Func<T, U>> CreateLambda<T>(MemberExpression propertyExpression, ParameterExpression parameter)
        => Expression.Lambda<Func<T, U>>(propertyExpression, parameter);
}