using FluentValidation;

namespace TendersApi.Application.Tenders.Queries.GetTenderById;

public sealed class GetTenderByIdQueryValidator : AbstractValidator<GetTenderByIdQuery>
{
    public GetTenderByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}