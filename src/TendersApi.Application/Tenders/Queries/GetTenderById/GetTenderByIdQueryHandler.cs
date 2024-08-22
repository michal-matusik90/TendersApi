using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Application.DTOs;
using TendersApi.Application.Interfaces;
using TendersApi.Application.Mappers;

namespace TendersApi.Application.Tenders.Queries.GetTenderById;

public sealed class GetTenderByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTenderByIdQuery, TenderDto?>
{
    public Task<TenderDto?> Handle(GetTenderByIdQuery request, CancellationToken cancellationToken)
        => context.Tenders
        .AsNoTracking()
        .Select(tender => TenderToTenderDtoMapper.Map(tender))
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
}
