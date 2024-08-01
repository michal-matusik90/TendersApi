using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Filters.Equality;

public sealed class EqualEqualityQueryableService : BaseEqualityQueryableService
{
    protected override Func<Expression, Expression, BinaryExpression> ComparisonExpressionFunc
        => Expression.Equal;

    public override bool CanHandle(FilterCriteria filterCriteria)
        => string.Equals(filterCriteria.Operator, nameof(Expression.Equal), StringComparison.OrdinalIgnoreCase);
}