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

public class TenderEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly TenderEndpoint _endpoint;
    private readonly FunctionContext _context;

    public TenderEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _endpoint = new TenderEndpoint(_mockMediator.Object, new Mock<ILogger<TenderEndpoint>>().Object);
        _context = new MockFunctionContext();
    }

    [Fact]
    public async Task GetTenderById_ShouldReturnNotFound_WhenSuppliersIsNotFound()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetTenderById.Query>(), default))
            .ReturnsAsync((TenderDto)null!);

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetTenderById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTenderById_ShouldReturnOk_WhenSupplierIsFound()
    {
        var dto = new TenderDto() { Id = "1234" };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetTenderById.Query>(), default))
            .ReturnsAsync(dto);

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetTenderById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await ReadResponseBodyAsync(response);
        var returnedDto = JsonConvert.DeserializeObject<TenderDto>(content);
        returnedDto.Should().NotBeNull();
        returnedDto!.Id.Should().Be("1234");
    }

    [Fact]
    public async Task GetTenderById_ShouldInternalServerError_WhenErrorOccure()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetTenderById.Query>(), default))
            .Throws<Exception>();

        var request = new MockHttpRequestData(_context, string.Empty);
        var response = await _endpoint.GetTenderById(request, "10", default);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponseData response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body);
        return await reader.ReadToEndAsync();
    }
}
