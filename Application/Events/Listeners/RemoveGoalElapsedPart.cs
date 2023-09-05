using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Models;
using Planner.Application.Services;
using Planner.Hubs;

namespace Planner.Application.Events.Listeners;

public class RemoveGoalElapsedPart : IListener<GoalElapsedPartRemoved>
{
	private readonly GoalsRepository _goalsRepository;
	private readonly IHubContext<TimersHub, ITimers> _hubContext;

	public RemoveGoalElapsedPart(GoalsRepository goalsRepository, IHubContext<TimersHub, ITimers> hubContext)
	{
		_goalsRepository = goalsRepository;
		_hubContext      = hubContext;
	}

	public async Task HandleAsync(GoalElapsedPartRemoved broadcasted)
    {
	    await _goalsRepository.Remove(broadcasted.Id);
	    ActiveGoal? goal = await _goalsRepository.One(broadcasted.GoalId, broadcasted.UserId);
	    if (goal is null) return;
	    await _hubContext.Clients.All.GoalElapsedTimePartRemoved(broadcasted.UserId, goal.Id, broadcasted.Id);
	    await _hubContext.Clients.All.GoalTimeChanged(broadcasted.UserId, goal.Id, goal.ElapsedTimeTotal);
	}
}