using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using TendersApi.Context;
using TendersApi.Models;
using TendersApi.Services;

namespace TendersApi.Endpoints;
public sealed class TendersSearchEndpoint(
    QueryableService queryableService,
    IValidator<SearchModelRequest> searchModelValidator,
    ApplicationDbContext context,
    ILogger<TendersSearchEndpoint> logger)
{
    [Function(nameof(TendersApi) + nameof(SearchApi))]
    public async Task<HttpResponseData> SearchApi(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tenders/search")]
        HttpRequestData request,
        CancellationToken cancellationToken)
    {
        try
        {
            var bodyContent = await request.ReadAsStringAsync();

            if (bodyContent is null)
            {
                return await BadRequest(request, "Request cannot be empty or null");
            }

            var searchModelRequest = JsonConvert.DeserializeObject<SearchModelRequest>(bodyContent);
            if (searchModelRequest is null)
            {
                return await BadRequest(request, "Invalid request");
            }

            var validationResult = await searchModelValidator.ValidateAsync(searchModelRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                var message = string.Join("\n", validationResult.Errors);
                return await BadRequest(request, message);
            }

            var tenders = queryableService.Build(context.Tenders, searchModelRequest).ToArray();
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

    private static async Task<HttpResponseData> BadRequest(HttpRequestData request, string message)
    {
        var response = request.CreateResponse(HttpStatusCode.BadRequest);

        await response.WriteStringAsync(message);

        return response;
    }
}
