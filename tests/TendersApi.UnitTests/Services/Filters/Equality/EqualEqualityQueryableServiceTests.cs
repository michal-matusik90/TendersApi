using Bogus;
using FluentAssertions;
using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;

namespace TendersApi.UnitTests.Services.Filters.Equality;

public sealed class EqualEqualityQueryableServiceTests
{
    private readonly EqualEqualityQueryableService _service;
    private readonly Faker _faker;

    public EqualEqualityQueryableServiceTests()
    {
        _service = new EqualEqualityQueryableService();
        _faker = new Faker();
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_WhenOperatorIsEqual()
    {
        var filterCriteria = new FilterCriteria { Operator = nameof(Expression.Equal) };
        _service.CanHandle(filterCriteria).Should().BeTrue();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_WhenOperatorIsNotEqual()
    {
        var filterCriteria = new FilterCriteria { Operator = "InvalidOperator" };
        _service.CanHandle(filterCriteria).Should().BeFalse();
    }

    [Fact]
    public void Handle_ShouldReturnEqualExpression_WhenCalled()
    {
        var filterCriteria = new FilterCriteria
        {
            Field = nameof(TestModel.Name),
            Value = _faker.Name.FirstName(),
            Operator = nameof(Expression.Equal)
        };

        var parameter = Expression.Parameter(typeof(TestModel), "entity");
        var expression = _service.Handle(parameter, filterCriteria);
        expression.Should().NotBeNull();
        expression.NodeType.Should().Be(ExpressionType.Equal);
    }

    private class TestModel
    {
        public string? Name { get; set; }
    }
}
