using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Models;
using Planner.Application.Services;
using Planner.Hubs;

namespace Planner.Application.Events.Listeners;

public class UpdateGoal : IListener<GoalChanged>
{
	private readonly GoalsRepository _goalsRepository;
	private readonly IHubContext<TimersHub, ITimers> _hubContext;

	public UpdateGoal(GoalsRepository goalsRepository, IHubContext<TimersHub, ITimers> hubContext)
	{
		_goalsRepository = goalsRepository;
		_hubContext = hubContext;
	}

    public async Task HandleAsync(GoalChanged broadcasted)
    {
	    await _goalsRepository.Update(broadcasted);
	    ActiveGoal? goal = await _goalsRepository.One(broadcasted.Id, broadcasted.UserId);
		if(goal is null) return;
		await _hubContext.Clients.All.GoalChanged(broadcasted.UserId, goal);
    }
}