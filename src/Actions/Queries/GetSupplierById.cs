using MediatR;
using Microsoft.EntityFrameworkCore;
using TendersApi.Context;
using TendersApi.Mappers;
using TendersApi.Models;

namespace TendersApi.Actions.Queries;

public sealed class GetSupplierById
{
    public sealed class Query : IRequest<TenderDto[]>
    {
        public string Id { get; init; } = default!;
    }

    public sealed class Handler(TendersContext context) : IRequestHandler<Query, TenderDto[]>
    {
        public Task<TenderDto[]> Handle(Query request, CancellationToken cancellationToken)
            => context.Tenders
            .AsNoTracking()
            .Where(tender => tender.Suppliers.Any(supplier => supplier.Id == request.Id))
            .Select(tender => TenderToTenderDtoMapper.Map(tender))
            .ToArrayAsync(cancellationToken);
    }
}