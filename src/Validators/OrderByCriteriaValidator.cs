using FluentValidation;
using TendersApi.Models;

namespace TendersApi.Validators;

public sealed class OrderByCriteriaValidator : AbstractValidator<OrderByCriteria>
{
    public OrderByCriteriaValidator()
    {
        var allowedFields = string.Join(", ", Enum.GetNames<OrderableFields>());
        var allowedDirections = string.Join(", ", Enum.GetNames<OrderDirection>());

        RuleFor(x => x.Field)
            .Must(x => Enum.TryParse<OrderableFields>(x, true, out var _))
            .WithMessage(x => $"Order By field '{x.Field}' is invalid. Allowed values are: {allowedFields}");

        RuleFor(x => x.Direction)
            .Must(x => Enum.TryParse<OrderDirection>(x, true, out var _))
            .WithMessage(x => $"Order By direction '{x.Field}' is invalid. Allowed values are: {allowedDirections}");
    }
}
