namespace TendersApi.Infrastructure.ExternalApi.TendersApi.Models;

public sealed class ExternalTendersApiConfiguration
{
    public string BaseAddress { get; set; }
    public TendersApiEndpoint Endpoint { get; set; }

    public sealed class TendersApiEndpoint
    {
        public string GetTenders { get; set; }
        public string GetTender { get; set; }
    }
}
