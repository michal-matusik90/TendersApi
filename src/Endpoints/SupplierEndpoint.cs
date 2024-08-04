using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using TendersApi.Actions.Queries;

namespace TendersApi.Endpoints;

public sealed class SupplierEndpoint(IMediator mediator, ILogger<SupplierEndpoint> logger)
{
    [Function(nameof(GetSupplierById))]
    public async Task<HttpResponseData> GetSupplierById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "suppliers/{id}")]
        HttpRequestData request,
        string id,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSupplierById.Query { Id = id };
            var tenders = await mediator.Send(query, cancellationToken);

            if (tenders is null || tenders.Length == 0)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonConvert.SerializeObject(tenders, Formatting.Indented), cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occured.");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
