﻿using Planner.Application.Database;
using Planner.Application.Events.Listeners;
using Planner.Application.Models;
using Planner.Application.Services.Auth;
using Planner.Application.Services;
using Spark.Library.Database;
using Spark.Library.Logging;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using Spark.Library.Auth;
using Planner.Application.Jobs;
using Spark.Library.Mail;

namespace Planner.Application.Startup;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCustomServices();
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddDatabase<DatabaseContext>(config);
        services.AddLogger(config);
        services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
        services.AddAuthentication<IAuthValidator>(config);
        services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
        services.AddJobServices();
        services.AddScheduler();
        services.AddQueue();
        services.AddEventServices();
        services.AddEvents();
        services.AddMailer(config);
        services.AddMudServices();
        services.AddMudMarkdownServices();
		return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // add custom services
        services.AddScoped<UsersService>();
        services.AddScoped<RolesService>();
        services.AddScoped<IAuthValidator, AuthValidator>();
        services.AddScoped<AuthService>();

        services.AddScoped<ContractorsRepository>();
        services.AddScoped<GoalsRepository>();
        services.AddScoped<JobsRepository>();
        services.AddScoped<UserSettingsRepository>();
        services.AddSingleton<TimeCounter>();

        return services;
    }

    private static IServiceCollection AddEventServices(this IServiceCollection services)
    {
        // add custom events here
        services.AddTransient<RemoveGoalElapsedPart>();
        services.AddTransient<UpdateGoal>();
        services.AddTransient<EmailNewUser>();
        services.AddTransient<BuildJob>();
        services.AddTransient<EditTimePartByHand>();
        return services;
    }

    private static IServiceCollection AddJobServices(this IServiceCollection services)
    {
        // add custom background tasks here
        services.AddTransient<TimeNotifier>();
        return services;
    }
}
