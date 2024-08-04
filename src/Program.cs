using Azure.Core.Serialization;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using TendersApi.Context;
using TendersApi.DependencyInjection;
using TendersApi.Mappers;
using TendersApi.Models;
using TendersApi.Services;
using TendersApi.Services.Filters;
using TendersApi.Services.Filters.Equality;
using TendersApi.Services.Filters.Logical;
using TendersApi.Services.Sorters;
using TendersApi.Validators;

var host = new HostBuilder()
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
    })
    .ConfigureFunctionsWebApplication((IFunctionsWorkerApplicationBuilder builder) =>
    {
        builder.Services.Configure<WorkerOptions>(workerOptions =>
        {
            var settings = NewtonsoftJsonObjectSerializer.CreateJsonSerializerSettings();

            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            workerOptions.Serializer = new NewtonsoftJsonObjectSerializer(settings);
        });
    })
    .ConfigureServices((context, services) =>
    {
        var databaseConnectionString = context.Configuration.GetConnectionString("TendersDatabase");

        services.AddScoped<IQueryableSubservice, FilterQueryableSubservice>();
        services.AddScoped<IEqualityQueryableService, GreaterThanEqualityQueryableService>();
        services.AddScoped<IEqualityQueryableService, LessThanEqualityQueryableService>();
        services.AddScoped<IEqualityQueryableService, EqualEqualityQueryableService>();

        services.AddScoped<ILogicalFilterQueryableService, AndFilterQueryableService>();
        services.AddScoped<ILogicalFilterQueryableService, OrFilterQueryableService>();
        services.AddScoped<IQueryableSubservice, OrderByQueryableSubservice>();
        services.AddScoped<IQueryableSubservice, TakeQueryableSubservice>();
        services.AddScoped<IQueryableSubservice, SkipQueryableSubservice>();

        services.AddScoped<IOrderDirectionQueryableService, AscendingQueryableService>();
        services.AddScoped<IOrderDirectionQueryableService, DescendingQueryableService>();

        services.AddScoped<QueryableService>();
        services.AddScoped<IValidator<PaginatedRequest>, PaginatedRequestValidator>();
        services.AddScoped<IValidator<SearchModelRequest>, SearchModelRequestValidator>();
        services.AddScoped<ExternalApiDataToTenderMapper>();
        services.AddOptions();
        services.AddExternalTendersApi(context.Configuration);
        services.AddDbContext<TendersContext>(options =>
        {
            options.UseSqlServer(databaseConnectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            });

        }, ServiceLifetime.Scoped);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
    })
    .Build();

await host.RunAsync();
