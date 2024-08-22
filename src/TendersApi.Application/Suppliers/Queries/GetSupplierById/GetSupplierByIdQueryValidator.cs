using FluentValidation;

namespace TendersApi.Application.Suppliers.Queries.GetSupplierById;

public sealed class GetSupplierByIdQueryValidator : AbstractValidator<GetSupplierByIdQuery>
{
    public GetSupplierByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}