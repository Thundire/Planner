using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Services;
using Planner.Hubs;

namespace Planner.Application.Events.Listeners;

public class EditTimePartByHand : IListener<TimePartEditedByHand>
{
	private readonly GoalsRepository _goalsRepository;
	private readonly IHubContext<TimersHub, ITimers> _hubContext;

	public EditTimePartByHand(GoalsRepository goalsRepository, IHubContext<TimersHub, ITimers> hubContext)
	{
		_goalsRepository = goalsRepository;
		_hubContext = hubContext;
	}

    public async Task HandleAsync(TimePartEditedByHand broadcasted)
    {
	    var changed = await _goalsRepository.ChangeTimeByHand(broadcasted);
	    await _hubContext.Clients.All.GoalTimePartTimeChanged(broadcasted.UserId, changed.Goal.Id, broadcasted.PartId, changed.ElapsedTime, changed.EditedByHand, changed.Comment);
	    var activeGoal = await _goalsRepository.One(changed.Goal.Id, broadcasted.UserId);
		if(activeGoal is null) return;
	    await _hubContext.Clients.All.GoalTimeChanged(broadcasted.UserId, activeGoal.Id, activeGoal.ElapsedTimeTotal);
    }
}