using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;

namespace TendersApi.Services.Filters.Logical;

public abstract class BaseLogicalQueryableService(IEnumerable<IEqualityQueryableService> filters) : ILogicalFilterQueryableService
{
    protected abstract Func<Expression, Expression, BinaryExpression> LogicalExpressionFunc { get; }

    public abstract bool CanHandle(string logicalOperator);

    public IQueryable<T> Handle<T>(IQueryable<T> query, IEnumerable<FilterCriteria> filterCriterias)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (filterCriterias is null || !filterCriterias.Any())
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T), "entity");
        var expressions = filterCriterias.Select(filterCriteria => RunHandler(parameter, filterCriteria));
        var finalExpression = expressions.First();

        foreach (var expression in expressions.Skip(1))
        {
            finalExpression = Expression.OrElse(finalExpression, expression);
        }

        var finalLambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return query.Where(finalLambda);
    }

    private Expression RunHandler(ParameterExpression parameter, FilterCriteria filterCriteria)
    {
        var handler = filters.SingleOrDefault(filter => filter.CanHandle(filterCriteria))
                ?? throw new InvalidOperationException("No handler found for the filter criteria.");

        return handler.Handle(parameter, filterCriteria);
    }
}
