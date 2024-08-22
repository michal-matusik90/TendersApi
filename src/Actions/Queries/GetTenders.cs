using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Context;
using TendersApi.Mappers;
using TendersApi.Models;

namespace TendersApi.Actions.Queries;

public sealed class GetTenders
{
    public sealed class Query : IRequest<TenderDto[]>
    {
        public int Skip { get; init; }
        public int Take { get; init; }
    }

    public sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, TenderDto[]>
    {
        public Task<TenderDto[]> Handle(Query request, CancellationToken cancellationToken)
            => context.Tenders
                .AsNoTracking()
                .Select(x => TenderToTenderDtoMapper.Map(x))
                .Skip(request.Skip)
                .Take(request.Take)
                .ToArrayAsync(cancellationToken);
    }
}
