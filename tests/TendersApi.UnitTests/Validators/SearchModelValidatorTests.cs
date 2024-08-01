using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using TendersApi.Models;
using TendersApi.Validators;

namespace TendersApi.UnitTests.Validators;

public sealed class SearchModelValidatorTests
{
    private readonly SearchModelValidator _validator;
    private readonly Faker _faker;

    public SearchModelValidatorTests()
    {
        _validator = new SearchModelValidator();
        _faker = new Faker();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(1_000_001)]
    public void Validate_ShouldHaveErrors_WhenTakeIsInvalid(int take)
    {
        var model = new SearchModel { Take = take };
        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.Take);
    }


    [Fact]
    public void Validate_ShouldNotHaveErrors_WhenTakeIsValid()
    {
        var model = new SearchModel { Take = _faker.Random.Int(1, 100) };
        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Take);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_ShouldHaveErrors_WhenSkipIsInvalid(int skip)
    {
        var model = new SearchModel { Skip = skip };
        _validator.TestValidate(model).ShouldHaveValidationErrorFor(x => x.Skip);
    }

    [Fact]
    public void Validate_ShouldNotHaveErrors_WhenSkipIsValid()
    {
        var model = new SearchModel { Skip = _faker.Random.Int(0, int.MaxValue) };
        _validator.TestValidate(model).ShouldNotHaveValidationErrorFor(x => x.Skip);
    }

    [Fact]
    public void Validate_ShouldHaveErrors_WhenFiltersAreInvalid()
    {
        var model = new SearchModel
        {
            Filters =
            [
                new() { Field = "InvalidField", Operator = "InvalidOperator" },
                new() { Field = "InvalidField", Operator = "InvalidOperator" }
            ],
            FilterCriteriaOperator = "InvalidOperator"
        };

        var result = _validator.TestValidate(model);
        result.Errors.Should().HaveCount(5);
    }

    [Fact]
    public void Validate_ShouldHaveErrors_WhenFilterIsInvalid()
    {
        var model = new SearchModel
        {
            Filters = [new() { Field = "InvalidField", Operator = "InvalidOperator" }],
            FilterCriteriaOperator = "InvalidOperator"
        };

        var result = _validator.TestValidate(model);
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Validate_ShouldNotHaveErrors_WhenFiltersAreValid()
    {
        var model = new SearchModel
        {
            Filters = [new() { Field = FilterableField.SupplierId.ToString(), Operator = EqualityOperator.Equal.ToString() }]
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Filters!.First().Field);
        result.ShouldNotHaveValidationErrorFor(x => x.Filters!.First().Operator);
        result.ShouldNotHaveValidationErrorFor(x => x.FilterCriteriaOperator);
    }

    [Fact]
    public void Validate_ShouldHaveErrors_WhenOrderByIsInvalid()
    {
        var model = new SearchModel
        {
            OrderBy = [new() { Field = "InvalidField", Direction = "InvalidDirection", Index = 1 }]
        };

        var result = _validator.TestValidate(model);
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Validate_ShouldNotHaveValidationErrors_WhenOrderByIsValid()
    {
        var model = new SearchModel
        {
            OrderBy = [new() { Field = OrderableFields.PriceEur.ToString(), Direction = OrderDirection.Ascending.ToString() }]
        };

        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveValidationErrorFor(x => x.OrderBy);
    }
}
