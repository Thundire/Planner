using Coravel.Events.Interfaces;

namespace Planner.Application.Events;

public class JobBuildRequested : IEvent
{
	public int UserId { get; set; }
	public DateTime RequestedAt { get; set; }
}