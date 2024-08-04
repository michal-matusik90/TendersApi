using FluentValidation;
using TendersApi.Models;

namespace TendersApi.Validators;

public sealed class SearchModelRequestValidator : AbstractValidator<SearchModelRequest>
{
    public SearchModelRequestValidator()
    {
        var allowedLogicalOperators = string.Join(", ", Enum.GetNames<LogicalOperator>());

        RuleFor(x => x.Take)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Take parameter must be greater than 0 and less than 100.");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Skip parameter must be greater than or equal to 0.");

        When(x => x.Filters is not null, () =>
        {
            RuleForEach(x => x.Filters).SetValidator(new FilterCriteriaValidator());
        });

        When(x => x.Filters is not null && x.Filters.Count() > 1, () =>
        {
            RuleFor(x => x.FilterCriteriaOperator)
               .Must(x => Enum.TryParse<LogicalOperator>(x, true, out var _))
               .WithMessage($"FilterCriteriaOperator must contain value when Filters are not null. Allowed values are: {allowedLogicalOperators}");
        });

        When(x => x.OrderBy is not null, () =>
        {
            RuleForEach(x => x.OrderBy).SetValidator(new OrderByCriteriaValidator());
        });
    }
}
