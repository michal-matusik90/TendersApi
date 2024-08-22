using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mime;
using System.Web.Http;
using TendersApi.Application.DTOs;
using TendersApi.Application.Tenders.Queries.GetTenderById;

namespace TendersApi.Functions;

public sealed class TenderEndpoint(IMediator mediator, ILogger<TenderEndpoint> logger)
{
    [FunctionName(nameof(GetTenderById))]
    [OpenApiOperation(operationId: nameof(GetTenderById), tags: ["id"])]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **id** of tender")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(TenderDto), Description = "Tender")]
    public async Task<IActionResult> GetTenderById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tenders/{id}")] HttpRequest req,
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetTenderByIdQuery { Id = id };
            var tender = await mediator.Send(query, cancellationToken);

            return tender is null
                ? new NotFoundResult()
                : new OkObjectResult(tender);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occured.");
            return new InternalServerErrorResult();
        }
    }
}