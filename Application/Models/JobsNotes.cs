using Spark.Library.Database;

namespace Planner.Application.Models;

public class JobsNotes : BaseModel
{
	public DateTime Date { get; set; }
	public User? User { get; set; }
	public List<JobsNote> Notes { get; set; } = new();
}