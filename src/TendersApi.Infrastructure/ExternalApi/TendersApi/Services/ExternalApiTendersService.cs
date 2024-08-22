using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TendersApi.Infrastructure.ExternalApi.TendersApi.Models;

namespace TendersApi.Infrastructure.ExternalApi.TendersApi.Services;

public sealed class ExternalApiTendersService(IHttpClientFactory httpClientFactory, IOptions<ExternalTendersApiConfiguration> apiConfiguration)
    : ITendersService
{
    public Task<HttpResponseMessage> GetTenders(int page, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(nameof(ExternalApiTendersService));
        var endpoint = apiConfiguration.Value.Endpoint.GetTenders;
        var param = new Dictionary<string, string?>()
        {
            [nameof(page)] = page.ToString(),
        };
        var uri = new Uri(QueryHelpers.AddQueryString(endpoint, param), UriKind.Relative);

        return httpClient.GetAsync(uri, cancellationToken);
    }
}
