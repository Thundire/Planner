using Spark.Library.Database;

namespace Planner.Application.Models;

public class GoalElapsedTimePart : BaseModel
{
    public string Comment { get; set; } = string.Empty;
    public TimeSpan ElapsedTime { get; set; }
    public bool Collapsed { get; set; }
    public virtual Goal Goal { get; set; } = new();
}