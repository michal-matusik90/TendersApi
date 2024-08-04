using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Filters.Equality;

public sealed class LessThanEqualityQueryableService : BaseEqualityQueryableService
{
    protected override Func<Expression, Expression, BinaryExpression> ComparisonExpressionFunc
        => Expression.LessThan;

    public override bool CanHandle(FilterCriteria filterCriteria)
        => string.Equals(filterCriteria.Operator, nameof(Expression.LessThan), StringComparison.OrdinalIgnoreCase);
}