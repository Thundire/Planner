using Coravel.Events.Interfaces;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Planner.Application.Events;
using Planner.Application.Jobs;
using Planner.Application.Models;
using Planner.Application.Services;

namespace Planner.Hubs;

public class TimersHub : Hub<ITimers>
{
	private readonly TimeCounter _timeCounter;
	private readonly GoalsRepository _goalsRepository;
	private readonly IQueue _queue;

	public TimersHub(TimeCounter timeCounter, GoalsRepository goalsRepository, IQueue queue)
	{
		_timeCounter     = timeCounter;
		_goalsRepository = goalsRepository;
		_queue      = queue;
	}
	
	public async Task<int> ActivateTimer(int id, int userId, DateTime startTime)
	{
		GoalElapsedTimePartStartData elapsedTimePartStartData = new(id, startTime);
		GoalElapsedTimePart elapsedTimePart = await _goalsRepository.Store(elapsedTimePartStartData);
		_timeCounter.Push(elapsedTimePart.Id, userId, startTime);
		return elapsedTimePart.Id;
	}

	public async Task<TimeSpan> StopTimer(int id, int userId, DateTime updateAt)
	{
		TimeSpan elapsedTimeTotal = _timeCounter.Stop(id, userId);
		if (elapsedTimeTotal != TimeSpan.Zero)
		{
			GoalElapsedTimePartData elapsedTimePartData = new(id, elapsedTimeTotal, updateAt);
			var elapsedTimePart = await _goalsRepository.Update(elapsedTimePartData);
			ActiveGoal? goal = await _goalsRepository.One(elapsedTimePart.Goal.Id, userId);
			if (goal is null) return elapsedTimeTotal;
			await Clients.All.GoalChanged(userId, goal);
		}
		return elapsedTimeTotal;
	}
}

public interface ITimers
{
	Task Tick(int id, int userId, TimeSpan elapsed);
	Task ActivateTimer(int id, int userId, DateTime startTime);
	Task StopTimer(int id, int userId);
	Task GoalChanged(int userId, ActiveGoal goal);
	Task GoalElapsedTimePartRemoved(int userId, int goalId, int elapsedTimePartId);
	Task GoalTimeChanged(int userId, int goalId, TimeSpan timeTotal);
}
