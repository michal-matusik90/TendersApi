using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;

namespace TendersApi.Services.Filters.Logical;

public sealed class AndFilterQueryableService(IEnumerable<IEqualityQueryableService> filters)
    : BaseLogicalQueryableService(filters)
{
    protected override Func<Expression, Expression, BinaryExpression> LogicalExpressionFunc
        => Expression.AndAlso;

    public override bool CanHandle(string logicalOperator)
        => string.Equals(logicalOperator, nameof(LogicalOperator.And), StringComparison.OrdinalIgnoreCase);
}