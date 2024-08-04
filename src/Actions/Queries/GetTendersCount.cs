using MediatR;
using TendersApi.Context;

namespace TendersApi.Actions.Queries;

public sealed class GetTendersCount
{
    public sealed class Query : IRequest<int>;

    public sealed class Handler(TendersContext context) : IRequestHandler<Query, int>
    {
        public Task<int> Handle(Query request, CancellationToken cancellationToken)
            => Task.FromResult(context.Tenders.Count());
    }
}
