using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Data;
using TendersApi.Models;

namespace TendersApi.Actions;

public sealed class GetTenderById
{
    public sealed class Query : IRequest<Tender?>
    {
        public string Id { get; init; } = default!;
    }

    public sealed class Handler(AppDbContext context) : IRequestHandler<Query, Tender?>
    {
        public Task<Tender?> Handle(Query request, CancellationToken cancellationToken)
            => context.Tenders.FirstOrDefaultAsync(x => x.TenderId == request.Id, cancellationToken);
    }
}
