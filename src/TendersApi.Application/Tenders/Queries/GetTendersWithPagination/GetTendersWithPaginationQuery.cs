using MediatR;
using TendersApi.Application.DTOs;
using TendersApi.Application.Models;

namespace TendersApi.Application.Tenders.Queries.GetTendersWithPagination;

public sealed class GetTendersWithPaginationQuery : IRequest<PaginatedReadOnlyCollection<TenderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

