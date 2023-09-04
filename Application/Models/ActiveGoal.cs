namespace Planner.Application.Models;

public class ActiveGoal
{
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }

	public int ElapsedTimePartId { get; set; }
	public TimeSpan ElapsedTimeTotal { get; set; }
	public TimeSpan ElapsedTime { get; set; }
	public bool ShowParts { get; set; }
	public bool Tick { get; set; }

	public ActiveGoal()
	{

	}

	public ActiveGoal(Goal source)
	{
		Id               = source.Id;
		CreatedAt        = source.CreatedAt;
		UpdatedAt        = source.UpdatedAt;
		Name             = source.Name;
		Contractor       = source.Contractor;
		Comment          = source.Comment;
		ElapsedTimeTotal = source.CollapseElapsedTime();
	}
}