using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using TendersApi.Data;
using TendersApi.Models;
using TendersApi.Services;

namespace TendersApi;

public sealed class TendersFunction(
    AppDbContext dbContext,
    IValidator<SearchModel> searchModelValidator,
    QueryableService queryableService,
    ILogger<TendersFunction> logger)
{
    [Function(nameof(TendersFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders")]
        HttpRequestData req,
        CancellationToken cancellationToken)
    {
        try
        {
            var search = new SearchModel();

            var validationResult = await searchModelValidator.ValidateAsync(search, cancellationToken);

            if (!validationResult.IsValid)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync(string.Join("\n", validationResult.Errors));
                return badRequestResponse;
            }

            var tenders = queryableService.Build(dbContext.Tenders, search).ToArray();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(tenders, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occured.");
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
