using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text;

namespace TendersApi.UnitTests.Mocks;

public sealed class MockHttpRequestData : HttpRequestData
{
    private readonly FunctionContext _context;

    public MockHttpRequestData(FunctionContext context) : base(context)
    {
        _context = context;
        Body = new MemoryStream();
        Url = new Uri("https://www.tenders.guru");
        Method = string.Empty;
        Cookies = [];
        Identities = [];
    }

    public MockHttpRequestData(FunctionContext context, string body) : this(context)
    {
        var bytes = Encoding.UTF8.GetBytes(body);
        Body = new MemoryStream(bytes);
    }

    public override Stream Body { get; }
    public override HttpHeadersCollection Headers { get; } = [];
    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
    public override Uri Url { get; }
    public override IEnumerable<ClaimsIdentity> Identities { get; }
    public override string Method { get; }

    public override HttpResponseData CreateResponse()
        => new MockHttpResponseData(_context);

    public void AddHeaderKeyVal(string key, string value)
    {
        Headers.Add(key, value);
    }

    public void AddQuery(string key, string value)
    {
        Query.Add(key, value);
    }
}
