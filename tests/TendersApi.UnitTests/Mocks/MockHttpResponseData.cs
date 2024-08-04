using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace TendersApi.UnitTests.Mocks;

public sealed class MockHttpResponseData(FunctionContext context) : HttpResponseData(context)
{
    public override HttpStatusCode StatusCode { get; set; }
    public override HttpHeadersCollection Headers { get; set; } = [];
    public override Stream Body { get; set; } = new MemoryStream();
    public override HttpCookies Cookies { get; } = default!;
}
