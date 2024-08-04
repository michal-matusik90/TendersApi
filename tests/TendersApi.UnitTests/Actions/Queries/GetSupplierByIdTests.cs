using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TendersApi.Actions.Queries;
using TendersApi.Context;
using TendersApi.Context.Models;

namespace TendersApi.UnitTests.Actions.Queries;

public class GetSupplierByIdTests
{
    private readonly TendersContext _context;
    private readonly GetSupplierById.Handler _handler;

    public GetSupplierByIdTests()
    {
        var options = new DbContextOptionsBuilder<TendersContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .EnableSensitiveDataLogging()
           .Options;

        _context = new TendersContext(options);
        _handler = new GetSupplierById.Handler(_context);
    }

    [Fact]
    public async Task Handle_ShouldReturnTenderDtos_WhenSupplierExists()
    {
        var supplierId = "1";
        var tender = new Tender
        {
            Id = "1",
            Suppliers = [new() { Id = supplierId, Name = "test" }]
        };

        _context.Tenders.Add(tender);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();

        var result = await _handler.Handle(new GetSupplierById.Query { Id = supplierId }, CancellationToken.None);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyArray_WhenSupplierDoesNotExist()
    {
        var supplierId = "non-existent-id";

        var result = await _handler.Handle(new GetSupplierById.Query { Id = supplierId }, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
