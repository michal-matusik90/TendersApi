
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;
using TendersApi.Actions.Queries;
using TendersApi.Endpoints;
using TendersApi.Models;
using TendersApi.UnitTests.Mocks;

namespace TendersApi.UnitTests.Endpoints;
public sealed class TendersEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly TendersEndpoint _endpoint;
    private readonly FunctionContext _context;
    private readonly Mock<IValidator<PaginatedRequest>> _validatorMock;

    public TendersEndpointTests()
    {
        _validatorMock = new Mock<IValidator<PaginatedRequest>>();
        _mockMediator = new Mock<IMediator>();
        _endpoint = new TendersEndpoint(_mockMediator.Object, _validatorMock.Object, new Mock<ILogger<TendersEndpoint>>().Object);
        _context = new MockFunctionContext();
    }

    [Fact]
    public async Task GetTenders_ShouldReturnBadRequest_WhenParametersAreInvalid()
    {
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("take", "invalid take"));

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PaginatedRequest>(), default))
            .ReturnsAsync(validationResult);

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetTenders(request, "10", "10", default);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await ReadResponseBodyAsync(response);
        content.Should().Contain("invalid take");
    }

    [Fact]
    public async Task GetTenders_ShouldReturnOk_WhenParametersAreValid()
    {
        var tenders = new TenderDto[] { new() };
        var validationResult = new FluentValidation.Results.ValidationResult();

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PaginatedRequest>(), default))
            .ReturnsAsync(validationResult);

        _mockMediator.Setup(x => x.Send(It.IsAny<GetTenders.Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenders);

        _mockMediator.Setup(x => x.Send(It.IsAny<GetTendersCount.Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var expectedDto = new TendersDto
        {
            Skip = 0,
            Take = 100,
            Tenders = tenders,
            Total = 1
        };

        var request = new MockHttpRequestData(_context);
        var response = await _endpoint.GetTenders(request, "null", "zebra", default);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await ReadResponseBodyAsync(response);
        content.Should().BeEquivalentTo(JsonConvert.SerializeObject(expectedDto, Formatting.Indented));
    }


    [Fact]
    public async Task GetTenders_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PaginatedRequest>(), default))
            .ThrowsAsync(new Exception());

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetTenders(request, "10", "10", default);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponseData response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body);
        return await reader.ReadToEndAsync();
    }

}
