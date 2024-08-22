using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Context;
using TendersApi.Mappers;
using TendersApi.Models;

namespace TendersApi.Actions.Queries;

public sealed class GetTenderById
{
    public sealed class Query : IRequest<TenderDto?>
    {
        public string Id { get; init; } = default!;
    }

    public sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, TenderDto?>
    {
        public Task<TenderDto?> Handle(Query request, CancellationToken cancellationToken)
            => context.Tenders
            .AsNoTracking()
            .Select(tender => TenderToTenderDtoMapper.Map(tender))
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}
