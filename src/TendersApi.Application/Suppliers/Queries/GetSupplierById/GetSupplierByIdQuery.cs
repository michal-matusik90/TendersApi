using MediatR;
using TendersApi.Application.DTOs;

namespace TendersApi.Application.Suppliers.Queries.GetSupplierById;

public sealed class GetSupplierByIdQuery : IRequest<TenderDto[]>
{
    public string Id { get; init; } = default!;
}
