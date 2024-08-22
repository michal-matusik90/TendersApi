using MediatR;
using TendersApi.Context;
using TendersApi.Context.Models;

namespace TendersApi.Actions.Commands;

public sealed class InsertTenders
{
    public sealed class Command : IRequest
    {
        public IEnumerable<Tender> Tenders { get; init; } = [];
    }

    public sealed class Handler(ApplicationDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            foreach (var tender in request.Tenders)
            {
                for (var i = 0; i < tender.Suppliers.Count; i++)
                {
                    var supplier = tender.Suppliers[i];
                    var dbSupplier = await context.Suppliers.FindAsync([supplier.Id], cancellationToken);
                    if (dbSupplier is null)
                    {
                        await context.Suppliers.AddAsync(supplier, cancellationToken);
                        continue;
                    }
                    tender.Suppliers[i] = dbSupplier;
                }

                var dbTender = await context.Tenders.FindAsync([tender.Id], cancellationToken);

                if (dbTender == null)
                {
                    await context.Tenders.AddAsync(tender, cancellationToken);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
