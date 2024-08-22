using MediatR;
using TendersApi.Application.DTOs;

namespace TendersApi.Application.Tenders.Queries.GetTenderById;

public sealed class GetTenderByIdQuery : IRequest<TenderDto?>
{
    public string Id { get; init; } = default!;
}
