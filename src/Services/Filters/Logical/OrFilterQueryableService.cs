using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;

namespace TendersApi.Services.Filters.Logical;

public sealed class OrFilterQueryableService(IEnumerable<IEqualityQueryableService> filters)
    : BaseLogicalQueryableService(filters)
{
    protected override Func<Expression, Expression, BinaryExpression> LogicalExpressionFunc
        => Expression.OrElse;

    public override bool CanHandle(string logicalOperator)
        => string.Equals(logicalOperator, nameof(LogicalOperator.Or), StringComparison.OrdinalIgnoreCase);
}
