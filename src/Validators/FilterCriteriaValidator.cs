using FluentValidation;
using TendersApi.Models;

namespace TendersApi.Validators;

public sealed class FilterCriteriaValidator : AbstractValidator<FilterCriteria>
{
    public FilterCriteriaValidator()
    {
        var allowedFields = string.Join(", ", Enum.GetNames<FilterableField>());
        var allowedOperators = string.Join(", ", Enum.GetNames<LogicalOperator>());

        RuleFor(x => x.Field)
            .Must(x => Enum.TryParse<FilterableField>(x, true, out var _))
            .WithMessage(x => $"Filter field value '{x.Field}' is invalid. Allowed values are: {allowedFields}");

        RuleFor(x => x.Operator)
            .Must(x => Enum.TryParse<LogicalOperator>(x, true, out var _))
            .WithMessage(x => $"Filter operator value '{x.Operator}' is invalid. Allowed values are: {allowedOperators}");
    }
}
