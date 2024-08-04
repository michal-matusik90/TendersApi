using FluentAssertions;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using TendersApi.Actions.Queries;
using TendersApi.Endpoints;
using TendersApi.Models;
using TendersApi.UnitTests.Mocks;

namespace TendersApi.UnitTests.Endpoints;

public class SupplierEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<SupplierEndpoint>> _mockLogger;
    private readonly SupplierEndpoint _endpoint;
    private readonly FunctionContext _context;

    public SupplierEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<SupplierEndpoint>>();
        _endpoint = new SupplierEndpoint(_mockMediator.Object, _mockLogger.Object);
        _context = new MockFunctionContext();
    }

    [Fact]
    public async Task GetSupplierById_ShouldReturnNotFound_WhenSuppliersIsNotFound()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetSupplierById.Query>(), default))
            .ReturnsAsync(Enumerable.Empty<TenderDto>().ToArray());

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetSupplierById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSupplierById_ShouldReturnOk_WhenSupplierIsFound()
    {
        var dto = new TenderDto() { Id = "1234" };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetSupplierById.Query>(), default))
            .ReturnsAsync([dto]);

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetSupplierById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await ReadResponseBodyAsync(response);
        var dtos = JsonConvert.DeserializeObject<TenderDto[]>(content);
        dtos.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetSupplierById_ShouldInternalServerError_WhenErrorOccure()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetSupplierById.Query>(), default))
            .Throws<Exception>();

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetSupplierById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponseData response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body);
        return await reader.ReadToEndAsync();
    }
}
