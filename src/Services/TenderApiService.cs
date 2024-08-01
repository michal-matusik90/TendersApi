namespace TendersApi.Services;

public sealed class TenderApiService(HttpClient httpClient, TenderApiConfiguration apiConfiguration)
{
    public Task<HttpResponseMessage> GetTenders(int skip, int take, CancellationToken cancellationToken)
    {
        var uri = apiConfiguration.Endpoint.GetTenders;
        return httpClient.GetAsync(uri, cancellationToken);
    }
}

public sealed class TenderApiConfiguration
{
    public string BaseAddress { get; init; } = "https://tenders.guru/api/pl/";
    public TenderApiEndpoint Endpoint { get; init; } = default!;
}

public sealed class TenderApiEndpoint
{
    public Uri GetTenders { get; init; } = new Uri("tenders/");
    public Uri GetTender { get; init; } = new Uri("tenders/{id}");
}