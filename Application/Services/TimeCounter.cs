using System.Collections.Concurrent;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Planner.Application.Services;

public class TimeCounter
{
	private ConcurrentDictionary<int, ConcurrentDictionary<int, ActiveGoalTimerData>> Timers { get; set; } = new();

	public void Push(int id, int userId, DateTime startTime)
	{
		Timers.AddOrUpdate(userId, _ =>
		{
			ConcurrentDictionary<int, ActiveGoalTimerData> dict = new();
			dict.TryAdd(id, new(startTime));
			return dict;
		}, (key, datas) =>
		{
			datas.GetOrAdd(id, _ => new(startTime));
			return datas;
		});
	}

	public void Remove(int id, int userId)
	{
		var timer = Timers.GetValueOrDefault(userId);
		if (timer != null)
		{
			timer.Remove(id, out _);
			if (timer.IsEmpty)
			{
				Timers.Remove(userId, out _);
			}
		}
	}

	public List<Elapsed> ListElapsedTime() => Timers.SelectMany(x => x.Value.Select(c=> new Elapsed(c.Key, x.Key, c.Value.ElapsedTime))).ToList();
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

public record Elapsed(int Id, int UserId, TimeSpan Time);