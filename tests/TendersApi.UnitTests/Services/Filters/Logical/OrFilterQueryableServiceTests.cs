using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;
using TendersApi.Services.Filters.Logical;

namespace TendersApi.UnitTests.Services.Filters.Logical;

public sealed class OrFilterQueryableServiceTests
{
    private readonly ICollection<IEqualityQueryableService> _filters = [];
    private readonly OrFilterQueryableService _service;

    public OrFilterQueryableServiceTests()
    {
        _service = new OrFilterQueryableService(_filters);
    }

    [Theory]
    [InlineData("Or", true)]
    [InlineData("NotOr", false)]
    public void CanHandle_ShouldReturnTrue_ForLogicalOperatorOr(string logicalOperator, bool expected)
    {
        _service.CanHandle(logicalOperator).Should().Be(expected);
    }

    [Fact]
    public void Handle_ShouldReturnOriginalQuery_WhenFilterCriteriasIsNull()
    {
        var query = CreateQueryable(2);
        _service.Handle(query, null!).Should().BeSameAs(query);
    }

    [Fact]
    public void Handle_ShouldReturnOriginalQuery_WhenFilterCriteriasIsEmpty()
    {
        var query = CreateQueryable(2);
        _service.Handle(query, []).Should().BeSameAs(query);
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenQueryIsNull()
    {
        var action = () => _service.Handle<TestEntity>(null!, []);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Handle_ShouldThrowInvalidOperationException_WhenNoHandlerIsFound()
    {
        var query = CreateQueryable(0);
        var action = () => _service.Handle(query, CreateFilterCriterias(1));

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenIEqualityQueryableServiceReturnsNull()
    {
        var mockFilter = new Mock<IEqualityQueryableService>();
        mockFilter
            .Setup(f => f.CanHandle(It.IsAny<FilterCriteria>()))
            .Returns(true);

        mockFilter
            .Setup(f => f.Handle(It.IsAny<ParameterExpression>(), It.IsAny<FilterCriteria>()))
            .Returns<Expression>(null!);

        _filters.Add(mockFilter.Object);

        var query = CreateQueryable(0);
        var action = () => _service.Handle(query, CreateFilterCriterias(2));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Handle_ShouldApplyOrFilter_WhenInputIsValid()
    {
        _filters.Add(new EqualEqualityQueryableService());
        var query = CreateQueryable();

        var result = _service.Handle(query, CreateFilterCriterias(2));

        result.ToList().Should().HaveCount(2);
        result.Should().NotBeNull();
        result.Should().NotBeSameAs(query);
    }

    private static IEnumerable<FilterCriteria> CreateFilterCriterias(int count)
        => Enumerable.Range(1, count++).Select(x => CreateFilterCriteria(x.ToString()));

    private static FilterCriteria CreateFilterCriteria(string id) => new()
    {
        Field = nameof(TestEntity.Id),
        Value = id,
        Operator = EqualityOperator.Equal.ToString(),
    };

    private static IQueryable<TestEntity> CreateQueryable(int count = 4)
        => Enumerable.Range(0, count).Select(x => new TestEntity { Id = x }).AsQueryable();

    private class TestEntity
    {
        public int Id { get; set; }
    }
}
