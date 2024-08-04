using Bogus;
using FluentAssertions;
using System.Linq.Expressions;
using TendersApi.Models;
using TendersApi.Services.Filters.Equality;

namespace TendersApi.UnitTests.Services.Filters.Equality;

public sealed class GreaterThanEqualityQueryableServiceTests
{
    private readonly GreaterThanEqualityQueryableService _service;
    private readonly Faker _faker;

    public GreaterThanEqualityQueryableServiceTests()
    {
        _service = new GreaterThanEqualityQueryableService();
        _faker = new Faker();
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_WhenOperatorIsGreaterThan()
    {
        var filterCriteria = new FilterCriteria { Operator = nameof(Expression.GreaterThan) };
        _service.CanHandle(filterCriteria).Should().BeTrue();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_WhenOperatorIsNotGreaterThan()
    {
        var filterCriteria = new FilterCriteria { Operator = "InvalidOperator" };
        _service.CanHandle(filterCriteria).Should().BeFalse();
    }

    [Fact]
    public void Handle_ShouldReturnGreaterThanExpression_WhenCalled()
    {
        var filterCriteria = new FilterCriteria
        {
            Field = nameof(TestModel.Age),
            Value = _faker.Random.Int().ToString(),
            Operator = nameof(Expression.GreaterThan)
        };

        var parameter = Expression.Parameter(typeof(TestModel), "entity");
        var expression = _service.Handle(parameter, filterCriteria);
        expression.Should().NotBeNull();
        expression.NodeType.Should().Be(ExpressionType.GreaterThan);
    }

    [Fact]
    public void Handle_ShouldReturnGreaterThanExpression_WhenCalled2()
    {
        var filterCriteria = new FilterCriteria
        {
            Field = nameof(TestModel.Birthday),
            Value = _faker.Date.PastDateOnly().ToString("yyyy-MM-dd"),
            Operator = nameof(Expression.GreaterThan)
        };

        var parameter = Expression.Parameter(typeof(TestModel), "entity");
        var expression = _service.Handle(parameter, filterCriteria);
        expression.Should().NotBeNull();
        expression.NodeType.Should().Be(ExpressionType.GreaterThan);
    }

    private sealed class TestModel
    {
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }
}
