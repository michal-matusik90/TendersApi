using FluentValidation;
using TendersApi.Models;

namespace TendersApi.Validators;

public sealed class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
{
    public PaginatedRequestValidator()
    {
        When(x => x.Skip is not null, () =>
        {
            RuleFor(x => x.Skip).Must(x => int.TryParse(x, out var _)).WithMessage(GetMessage(nameof(PaginatedRequest.Skip)));
            When(x => int.TryParse(x.Skip, out var _), () =>
            {
                RuleFor(x => int.Parse(x.Skip!))
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(GetMessage(nameof(PaginatedRequest.Skip)));
            });
        });

        When(x => x.Take is not null, () =>
        {
            RuleFor(x => x.Take).Must(x => int.TryParse(x, out var _)).WithMessage(GetMessage(nameof(PaginatedRequest.Take)));
            When(x => int.TryParse(x.Take, out var _), () =>
            {
                RuleFor(x => int.Parse(x.Take!))
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(GetMessage(nameof(PaginatedRequest.Take)));
            });
        });
    }

    private static string GetMessage(string parameter)
    {
        return $"Parameter '{parameter}' must be integer and greater than or equal to 0";
    }
}
