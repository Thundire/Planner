using Spark.Library.Database;

namespace Planner.Application.Models;

public class JobsNote : BaseModel
{
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public TimeSpan Time { get; set; }
	public bool Completed { get; set; }
	public Contractor? Contractor { get; set; }
}