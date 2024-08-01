using System.Linq.Expressions;
using TendersApi.Models;

namespace TendersApi.Services.Filters.Equality;

public interface IEqualityQueryableService
{
    public bool CanHandle(FilterCriteria filterCriteria);
    public Expression Handle(ParameterExpression parameter, FilterCriteria filterCriteria);
}