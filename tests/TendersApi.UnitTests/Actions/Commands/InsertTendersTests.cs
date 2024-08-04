using Microsoft.EntityFrameworkCore;
using Moq;
using TendersApi.Actions.Commands;
using TendersApi.Context;
using TendersApi.Context.Models;

namespace TendersApi.UnitTests.Actions.Commands;

public class InsertTendersTests
{
    private readonly Mock<DbSet<Tender>> _mockTenderSet;
    private readonly Mock<DbSet<Supplier>> _mockSupplierSet;
    private readonly Mock<TendersContext> _mockContext;
    private readonly InsertTenders.Handler _handler;

    public InsertTendersTests()
    {
        _mockTenderSet = new Mock<DbSet<Tender>>();
        _mockSupplierSet = new Mock<DbSet<Supplier>>();
        _mockContext = new Mock<TendersContext>();

        _mockContext.Setup(m => m.Tenders).Returns(_mockTenderSet.Object);
        _mockContext.Setup(m => m.Suppliers).Returns(_mockSupplierSet.Object);

        _handler = new InsertTenders.Handler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddTendersAndSuppliers_WhenTheyDoNotExist()
    {
        var command = CreateCommand();

        _mockSupplierSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier)null!);

        _mockTenderSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tender)null!);

        await _handler.Handle(command, CancellationToken.None);

        _mockSupplierSet.Verify(m => m.AddAsync(It.IsAny<Supplier>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockTenderSet.Verify(m => m.AddAsync(It.IsAny<Tender>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotAddSupplier_WhenSupplierAlreadyExists()
    {
        var command = CreateCommand();

        _mockSupplierSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(command.Tenders.First().Suppliers.First());

        _mockTenderSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tender)null!);

        await _handler.Handle(command, CancellationToken.None);

        _mockSupplierSet.Verify(m => m.AddAsync(It.IsAny<Supplier>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockTenderSet.Verify(m => m.AddAsync(It.IsAny<Tender>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotAddTender_WhenTenderAlreadyExists()
    {
        var command = CreateCommand();

        _mockSupplierSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier)null!);

        _mockTenderSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(command.Tenders.First());

        await _handler.Handle(command, CancellationToken.None);

        _mockSupplierSet.Verify(m => m.AddAsync(It.IsAny<Supplier>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockTenderSet.Verify(m => m.AddAsync(It.IsAny<Tender>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static InsertTenders.Command CreateCommand()
    {
        var tender = new Tender
        {
            Id = "1",
            Suppliers = [new() { Id = "1", Name = "Supplier1" }]
        };

        return new InsertTenders.Command { Tenders = [tender] };
    }
}
