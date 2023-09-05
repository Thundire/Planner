using Coravel.Events.Interfaces;
using Planner.Application.Models;

namespace Planner.Application.Events;

public class GoalChanged : IEvent
{
	private readonly Contractor? _contractor;
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public Contractor? Contractor
	{
		get => _contractor;
		init => _contractor = Equals(value, Contractor.Empty) ? null : value;
	}

	public string Comment { get; init; } = string.Empty;
	public DateTime UpdatedAt { get; set; }
	public int UserId { get; set; }
}