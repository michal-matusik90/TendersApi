using System.Globalization;
using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Filters.Equality;

public abstract class BaseEqualityQueryableService : IEqualityQueryableService
{
    protected abstract Func<Expression, Expression, BinaryExpression> ComparisonExpressionFunc { get; }

    public abstract bool CanHandle(FilterCriteria filterCriteria);

    public Expression Handle(ParameterExpression parameter, FilterCriteria filterCriteria)
    {
        var member = Expression.PropertyOrField(parameter, filterCriteria.Field);
        var typedValue = Convert.ChangeType(filterCriteria.Value, member.Type, CultureInfo.InvariantCulture);
        var value = Expression.Constant(typedValue, member.Type);

        return ComparisonExpressionFunc(member, value);
    }
}