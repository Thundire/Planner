using Planner.Application.Jobs;
using Coravel;

namespace Planner.Application.Startup;

public static class Scheduler
{
    public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
    {
        services.UseScheduler(scheduler =>
        {
            // example scheduled job
            scheduler
                .Schedule<TimeNotifier>()
                .EverySecond()
                .PreventOverlapping(nameof(TimeNotifier));
        });
        return services;
    }
}
