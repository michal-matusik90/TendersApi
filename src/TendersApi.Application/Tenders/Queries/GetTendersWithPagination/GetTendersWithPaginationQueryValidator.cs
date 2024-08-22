using FluentValidation;

namespace TendersApi.Application.Tenders.Queries.GetTendersWithPagination;

public sealed class GetTendersWithPaginationQueryValidator : AbstractValidator<GetTendersWithPaginationQuery>
{
    public GetTendersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
