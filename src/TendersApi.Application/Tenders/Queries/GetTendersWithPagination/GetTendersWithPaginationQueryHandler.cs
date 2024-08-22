using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Application.DTOs;
using TendersApi.Application.Extensions;
using TendersApi.Application.Interfaces;
using TendersApi.Application.Mappers;
using TendersApi.Application.Models;

namespace TendersApi.Application.Tenders.Queries.GetTendersWithPagination;

public sealed class GetTendersWithPaginationQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTendersWithPaginationQuery, PaginatedReadOnlyCollection<TenderDto>>
{
    public Task<PaginatedReadOnlyCollection<TenderDto>> Handle(GetTendersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return context.Tenders
            .AsNoTracking()
            .Select(tender => TenderToTenderDtoMapper.Map(tender))
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}

