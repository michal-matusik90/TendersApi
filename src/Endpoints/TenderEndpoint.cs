using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using TendersApi.Actions.Queries;

namespace TendersApi.Endpoints;

public sealed class TenderEndpoint(IMediator mediator, ILogger<TendersEndpoint> logger)
{
    [Function(nameof(GetTenderById))]
    public async Task<HttpResponseData> GetTenderById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders/{id}")]
        HttpRequestData request,
        string id,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetTenderById.Query { Id = id };
            var tender = await mediator.Send(query, cancellationToken);

            if (tender is null)
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonConvert.SerializeObject(tender, Formatting.Indented), cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occured.");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
