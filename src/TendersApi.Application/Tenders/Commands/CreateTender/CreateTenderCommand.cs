using MediatR;
using TendersApi.Domain.Entities;

namespace TendersApi.Application.Tenders.Commands.CreateTender;

public sealed class CreateTenderCommand : IRequest
{
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public decimal Value { get; set; }
    public decimal ValueEur { get; set; }
    public Supplier[] Suppliers { get; set; } = [];
}
