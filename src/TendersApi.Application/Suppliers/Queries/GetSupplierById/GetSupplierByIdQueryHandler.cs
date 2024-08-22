namespace TendersApi.Application.Suppliers.Queries.GetSupplierById;
/*
public sealed class GetSupplierByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSupplierByIdQuery, TenderDto[]>
{
    public Task<TenderDto[]> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        => context.Suppliers
        .AsNoTracking()
        .Where(x => x.Id == request.Id)
        .Select(x => )
        .Where(tender => tender.TenderSuppliers.Any(supplier => supplier.Id == request.Id))
        .Select(tender => TenderToTenderDtoMapper.Map(tender))
        .ToArrayAsync(cancellationToken);
}
*/