using FluentValidation.TestHelper;
using TendersApi.Models;
using TendersApi.Validators;

namespace TendersApi.UnitTests.Validators;

public class PaginatedRequestValidatorTests
{
    private readonly PaginatedRequestValidator _validator;

    public PaginatedRequestValidatorTests()
    {
        _validator = new PaginatedRequestValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("0")]
    [InlineData("10")]
    public void Validate_ShouldNotHaveErrors_WhenSkipIsValid(string? skip)
    {
        var model = new PaginatedRequest { Skip = skip };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Skip);
    }

    [Theory]
    [InlineData("")]
    [InlineData("-1")]
    [InlineData("abc")]
    [InlineData(" ")]
    public void Validate_ShouldHaveErrors_WhenSkipIsInvalid(string? skip)
    {
        var model = new PaginatedRequest { Skip = skip };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Skip);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("0")]
    [InlineData("10")]
    public void Validate_ShouldNotHaveErrors_WhenTakeIsValid(string? take)
    {
        var model = new PaginatedRequest { Take = take };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Take);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("abc")]
    [InlineData(" ")]
    [InlineData("")]
    public void Validate_ShouldHaveErrors_WhenTakeIsInvalid(string? take)
    {
        var model = new PaginatedRequest { Take = take };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Take);
    }
}
