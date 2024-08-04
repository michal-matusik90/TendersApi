using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using TendersApi.Actions.Queries;
using TendersApi.Models;

namespace TendersApi.Endpoints;

public sealed class TendersEndpoint(
    IMediator mediator,
    IValidator<PaginatedRequest> paginatedRequestValidator,
    ILogger<TendersEndpoint> logger)
{
    [Function(nameof(TendersApi) + nameof(GetTenders))]
    public async Task<HttpResponseData> GetTenders(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders")]
        HttpRequestData request,
        string? skip,
        string? take,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestBody = new PaginatedRequest
            {
                Skip = skip,
                Take = take,
            };

            var validationResult = await paginatedRequestValidator.ValidateAsync(requestBody, cancellationToken);

            if (!validationResult.IsValid)
            {
                var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);

                await badRequestResponse.WriteStringAsync(string.Join(", ", validationResult.Errors), cancellationToken);

                return badRequestResponse;
            }

            var query = new GetTenders.Query()
            {
                Skip = ParseOrDefault(skip, 0),
                Take = ParseOrDefault(take, 100),
            };

            var tenders = await mediator.Send(query, cancellationToken);
            var tendersCountQuery = new GetTendersCount.Query();
            var tendersCount = await mediator.Send(tendersCountQuery, cancellationToken);

            var responseBody = new TendersDto
            {
                Skip = query.Skip,
                Take = query.Take,
                Total = tendersCount,
                Tenders = tenders
            };

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonConvert.SerializeObject(responseBody, Formatting.Indented), cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occured.");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    private static int ParseOrDefault(string? value, int defaultValue)
    {
        if (int.TryParse(value, out var result))
        {
            return result;
        }
        return defaultValue;
    }
}
