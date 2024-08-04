using System.Linq.Expressions;

namespace TendersApi.Services.Sorters;

public interface IOrderByPropertyTypeService
{
    public bool CanSort(Type type);
    public IOrderedQueryable<T> OrderByAscending<T>(IQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter);
    public IOrderedQueryable<T> OrderByAscending<T>(IOrderedQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter);
    public IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter);
    public IOrderedQueryable<T> OrderByDescending<T>(IOrderedQueryable<T> query, MemberExpression propertyExpression, ParameterExpression parameter);
}