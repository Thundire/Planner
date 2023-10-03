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
	private readonly JobsRepository _jobsRepository;

	public TimersHub(TimeCounter timeCounter, GoalsRepository goalsRepository, IQueue queue, JobsRepository jobsRepository)
	{
		_timeCounter         = timeCounter;
		_goalsRepository     = goalsRepository;
		_queue               = queue;
		_jobsRepository = jobsRepository;
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
			ActiveGoal? goal = await _goalsRepository.StopTimer(elapsedTimePartData, userId);
			if (goal is null) return elapsedTimeTotal;
			await Clients.All.TimerStopped(goal.Id, userId, goal.ElapsedTimeTotal);
		}
		return elapsedTimeTotal;
	}

	public async Task SetJobStatus(int userId, int jobsNotesId, int jobsNoteId, bool status)
	{
		await _jobsRepository.SetJobsStatus(jobsNoteId, status);
		await Clients.All.JobStatusChanged(userId, jobsNotesId, jobsNoteId, status);
	}
}

public interface ITimers
{
	Task Tick(int id, int userId, TimeSpan elapsed);
	Task ActivateTimer(int id, int userId, DateTime startTime);
	Task StopTimer(int id, int userId);
	Task TimerStopped(int id, int userId, TimeSpan timeTotal);
	Task GoalChanged(int userId, ActiveGoal goal);
	Task GoalElapsedTimePartRemoved(int userId, int goalId, int elapsedTimePartId);
	Task GoalTimeChanged(int userId, int goalId, TimeSpan timeTotal);
	Task GoalTimePartTimeChanged(int userId, int goalId, int goalPartId, TimeSpan time);
	Task JobBuild(int userId, JobsNotes jobsNotes);
	Task SetJobStatus(int userId, int jobsNotesId, int jobsNoteId, bool status);
	Task JobStatusChanged(int userId, int jobsNotesId, int jobsNoteId, bool status);
	Task GoalTimePartTimeChanged(int userId, int goalId, int goalPartId, TimeSpan time, bool editedByHand, string comment);
}
