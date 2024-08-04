using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TendersApi.Models;

namespace TendersApi.Services;

public sealed class ExternalTendersApiService(IHttpClientFactory httpClientFactory, IOptions<ExternalTendersApiConfiguration> apiConfiguration)
{
    public Task<HttpResponseMessage> GetTenders(int page, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(nameof(ExternalTendersApiService));
        var endpoint = apiConfiguration.Value.Endpoint.GetTenders;
        var param = new Dictionary<string, string?>()
        {
            [nameof(page)] = page.ToString(),
        };
        var uri = new Uri(QueryHelpers.AddQueryString(endpoint, param), UriKind.Relative);

        return httpClient.GetAsync(uri, cancellationToken);
    }
}
