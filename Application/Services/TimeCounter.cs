using System.Collections.Concurrent;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Planner.Application.Services;

public class TimeCounter
{
	private ConcurrentDictionary<int, ActiveGoalTimerData> Timers { get; set; } = new();

	public void Push(int id, DateTime startTime)
	{
		_ = Timers.GetOrAdd(id, _ => new(startTime));
	}

	public void Remove(int id) => Timers.Remove(id, out _);

	public List<Elapsed> ListElapsedTime() => Timers.Select(x => new Elapsed(x.Key, x.Value.ElapsedTime)).ToList();
}

public class ActiveGoalTimerData
{
	public ActiveGoalTimerData(DateTime startTime)
	{
		StartTime     =  startTime;
		Timer         =  new(1) { AutoReset = true };
		Timer.Elapsed += TimerOnElapsed;
	}

	private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		ElapsedTime = e.SignalTime.Subtract(StartTime);
	}

	public DateTime StartTime { get; }
	public TimeSpan ElapsedTime { get; private set; }
	public Timer Timer { get; } 
}

public record Elapsed(int Id, TimeSpan Time);