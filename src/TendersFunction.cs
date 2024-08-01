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
    [Function(nameof(TendersFunction) + nameof(SearchApi))]
    public async Task<HttpResponseData> SearchApi(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tenders/search")]
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

    [Function(nameof(SearchApi) + nameof(GetTenders))]
    public Task<HttpResponseData> GetTenders(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders")]
        HttpRequestData req,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Function(nameof(SearchApi) + nameof(GetTenderById))]
    public Task<HttpResponseData> GetTenderById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders/{id}")]
        HttpRequestData req,
        string id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Function(nameof(SearchApi) + nameof(GetTenderById))]
    public Task<HttpResponseData> PullTenders(
        [TimerTrigger("0 0 * * * *", RunOnStartup = true)]
        TimerInfo timer,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
