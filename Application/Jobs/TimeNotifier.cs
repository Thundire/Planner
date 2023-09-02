using Coravel.Invocable;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Services;
using Planner.Hubs;

namespace Planner.Application.Jobs;

public class TimeNotifier : IInvocable
{
	private readonly TimeCounter _timeCounter;
	private readonly IHubContext<TimersHub, ITimers> _timersHubContext;

	public TimeNotifier(TimeCounter timeCounter, IHubContext<TimersHub, ITimers> timersHubContext)
	{
		_timeCounter           = timeCounter;
		_timersHubContext = timersHubContext;
	}

    public Task Invoke()
    {
	    var elapsedTimes = _timeCounter.ListElapsedTime();
	    Parallel.ForEachAsync(elapsedTimes, async (elapsed, token) =>
	    {
		    await _timersHubContext.Clients.All.Tick(elapsed.Id, elapsed.UserId, elapsed.Time);
	    });
        return Task.CompletedTask;
    }
}
