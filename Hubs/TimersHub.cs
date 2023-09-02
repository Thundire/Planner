using Microsoft.AspNetCore.SignalR;

namespace Planner.Hubs;

public class TimersHub : Hub<ITimers>
{
	public async Task Tick(int id, int userId, TimeSpan elapsed)
	{
		await Clients.All.Tick(id, userId, elapsed);
	}

	public async Task ActivateTimer(int id, int userId)
	{

	}

	public async Task StopTimer(int id, int userId)
	{

	}
}

public interface ITimers
{
	Task Tick(int id, int userId, TimeSpan elapsed);
}
