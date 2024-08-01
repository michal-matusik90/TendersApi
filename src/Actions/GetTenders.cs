using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Data;

namespace TendersApi.Actions;

public sealed class GetTenders
{
    public sealed class Query : IRequest
    {
        public int Skip { get; init; }
        public int Take { get; init; }
    }

    public sealed class Handler(AppDbContext context) : IRequestHandler<Query>
    {
        public Task Handle(Query request, CancellationToken cancellationToken)
            => context.Tenders
                .Skip(request.Skip)
                .Take(request.Take)
                .ToArrayAsync(cancellationToken);
    }
}
