using MediatR;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TendersApi.Actions.Commands;
using TendersApi.Mappers;
using TendersApi.Models;
using TendersApi.Services;

namespace TendersApi;

public sealed class TimerFunction
{
    private readonly ExternalTendersApiService _tenderApiService;
    private readonly ExternalApiDataToTenderMapper _mapper;
    private readonly IMediator _mediator;
    private readonly JsonSerializerSettings _settings;

    public TimerFunction(ExternalTendersApiService tenderApiService, ExternalApiDataToTenderMapper mapper, IMediator mediator)
    {
        _settings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() }
        };

        _tenderApiService = tenderApiService;
        _mapper = mapper;
        _mediator = mediator;
    }

    //[Function(nameof(TimerFunction) + nameof(PullTenders))]
    public async Task PullTenders(
        [TimerTrigger("0 0 0 * * *", RunOnStartup = false)]
        TimerInfo timer,
        CancellationToken cancellationToken)
    {
        var tasks = Enumerable.Range(1, 100)
            .Select(page => _tenderApiService.GetTenders(page, cancellationToken))
            .ToList();

        while (tasks.Count != 0)
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            var pagedResult = await finishedTask;

            await HandlePagedResult(pagedResult, cancellationToken);
        }
    }

    private async Task HandlePagedResult(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var tenders = JsonConvert.DeserializeObject<ExternalResponse>(content, _settings)
            ?? throw new JsonSerializationException();

        var models = tenders.Data.Select(_mapper.Map);

        await _mediator.Send(new InsertTenders.Command { Tenders = models }, cancellationToken);
    }
}