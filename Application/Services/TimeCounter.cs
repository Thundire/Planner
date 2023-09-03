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

	public TimeSpan Stop(int id, int userId)
	{
		var timer = Timers.GetValueOrDefault(userId);
		if (timer != null)
		{
			if (timer.Remove(id, out var timerData))
			{
				timerData.Dispose();
				return timerData.ElapsedTime;
			}

			if (timer.IsEmpty)
			{
				Timers.Remove(userId, out _);
			}
		}
		return TimeSpan.Zero;
	}

	public List<Elapsed> ListElapsedTime() => Timers.SelectMany(x => x.Value.Select(c=> new Elapsed(c.Key, x.Key, c.Value.ElapsedTime))).ToList();
}

public class ActiveGoalTimerData : IDisposable
{
	public ActiveGoalTimerData(DateTime startTime)
	{
		StartTime     =  startTime;
		Timer         =  new(1) { AutoReset = true };
		Timer.Elapsed += TimerOnElapsed;
		Timer.Start();
	}

	private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		ElapsedTime = e.SignalTime.Subtract(StartTime);
	}

	public DateTime StartTime { get; }
	public TimeSpan ElapsedTime { get; private set; }
	public Timer Timer { get; }

	public void Dispose()
	{
		Timer.Dispose();
	}
}

public record Elapsed(int Id, int UserId, TimeSpan Time);