using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Filters.Equality;

public sealed class GreaterThanEqualityQueryableService : BaseEqualityQueryableService
{
    protected override Func<Expression, Expression, BinaryExpression> ComparisonExpressionFunc
        => Expression.GreaterThan;

    public override bool CanHandle(FilterCriteria filterCriteria)
        => string.Equals(filterCriteria.Operator, nameof(Expression.GreaterThan), StringComparison.OrdinalIgnoreCase);
}