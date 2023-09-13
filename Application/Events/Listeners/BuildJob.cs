using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Models;
using Planner.Application.Services;
using Planner.Hubs;

namespace Planner.Application.Events.Listeners;

public class BuildJob : IListener<JobBuildRequested>
{
	private readonly JobsRepository _jobsRepository;
	private readonly IHubContext<TimersHub, ITimers> _hubContext;
	private readonly TimeCounter _timeCounter;
	private readonly GoalsRepository _goalsRepository;

	public BuildJob(JobsRepository jobsRepository, IHubContext<TimersHub, ITimers> hubContext, TimeCounter timeCounter, GoalsRepository goalsRepository)
	{
		_jobsRepository       = jobsRepository;
		_hubContext           = hubContext;
		_timeCounter          = timeCounter;
		_goalsRepository = goalsRepository;
	}

	public async Task HandleAsync(JobBuildRequested broadcasted)
	{
		var elapsed = _timeCounter.Stop(broadcasted.UserId);
		var elapsedParts = elapsed.Select(x => new GoalElapsedTimePart() { Id = x.Id, ElapsedTime = x.Time, UpdatedAt = broadcasted.RequestedAt }).ToList();
		var affectedGoals = await _goalsRepository.StopTimers(elapsedParts);
		await Parallel.ForEachAsync(affectedGoals, async (goal, token) =>
		{
			await _hubContext.Clients.All.GoalChanged(broadcasted.UserId, goal);
		});
		var notes = await _jobsRepository.Build(broadcasted.UserId, broadcasted.RequestedAt);
		await _hubContext.Clients.All.JobBuild(broadcasted.UserId, notes);
	}
}