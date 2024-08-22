using MediatR;
using TendersApi.Application.Interfaces;
using TendersApi.Domain.Entities;

namespace TendersApi.Application.Tenders.Commands.CreateTender;

public sealed class CreateTenderCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTenderCommand>
{
    public async Task Handle(CreateTenderCommand request, CancellationToken cancellationToken)
    {
        var tender = new Tender
        {

        };

        await context.Tenders.AddAsync(tender, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
