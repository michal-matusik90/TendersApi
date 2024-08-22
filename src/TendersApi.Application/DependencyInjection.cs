﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TendersApi.Application.Behaviours;

namespace TendersApi.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        });
    }
}
