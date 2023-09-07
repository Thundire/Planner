using Spark.Library.Database;

namespace Planner.Application.Models;

public class UserSettings : BaseModel
{
	public User? User { get; set; }
	public string TimeFormatter { get; set; } = string.Empty;
	public string DetailedTimeFormatter { get; set; } = string.Empty;
}