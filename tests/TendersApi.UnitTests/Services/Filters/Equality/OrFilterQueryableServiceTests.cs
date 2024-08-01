using FluentAssertions;
using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;
using TendersApi.Services.Filters.Logical;

namespace TendersApi.UnitTests.Services.Filters.Equality;

public sealed class OrFilterQueryableServiceTests
{
    private readonly List<IEqualityQueryableService> _mockFilters = [];
    private readonly OrFilterQueryableService _service;

    public OrFilterQueryableServiceTests()
    {
        _service = new OrFilterQueryableService(_mockFilters);
    }

    [Theory]
    [InlineData("Or", true)]
    [InlineData("NotOr", false)]
    public void CanHandle_ShouldReturnTrue_ForLogicalOperatorOr(string logicalOperator, bool expected)
    {
        _service.CanHandle(logicalOperator).Should().Be(expected);
    }

    [Fact]
    public void Handle_ShouldReturnOriginalQuery_WhenFilterCriteriasIsNullOrEmpty()
    {
        var query = new List<TestEntity>().AsQueryable();
        _service.Handle(query, null!).Should().BeSameAs(query);
    }

    [Fact]
    public void Handle_ShouldApplyOrFilter()
    {
        var filterCriteria1 = new FilterCriteria()
        {
            Field = nameof(TestEntity.Id),
            Value = "1",
            Operator = EqualityOperator.Equal.ToString(),
        };

        var filterCriteria2 = new FilterCriteria()
        {
            Field = nameof(TestEntity.Id),
            Value = "2",
            Operator = EqualityOperator.Equal.ToString(),
        };

        var parameter = Expression.Parameter(typeof(TestEntity), "entity");
        var filterService1 = new FakeEqualEqualityQueryableService(1);
        var filterService2 = new FakeEqualEqualityQueryableService(2);

        var filterServices = new List<IEqualityQueryableService> { filterService1, filterService2 };
        var service = new OrFilterQueryableService(filterServices);

        var query = new TestEntity[]
        {
            new() { Id = 1, Name = "Donatello" },
            new() { Id = 2, Name = "Michalaneglo" },
            new() { Id = 3, Name = "Donatello" },
            new() { Id = 4, Name = "Raphaello" },
        }.AsQueryable();

        var result = service.Handle(query, [filterCriteria1, filterCriteria2]);

        result.ToList().Should().HaveCount(2);
        result.Should().NotBeNull();
        result.Should().NotBeSameAs(query);
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private sealed class FakeEqualEqualityQueryableService(int expectedId) : IEqualityQueryableService
    {
        public bool CanHandle(FilterCriteria filterCriteria)
            => int.Parse(filterCriteria.Value!) == expectedId;

        public Expression Handle(ParameterExpression parameter, FilterCriteria filterCriteria)
        {
            return Expression.Equal(
                Expression.Property(parameter, typeof(TestEntity).GetProperty(nameof(TestEntity.Id))!),
                Expression.Constant(expectedId)
            );
        }
    }
}
