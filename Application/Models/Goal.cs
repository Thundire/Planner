using Spark.Library.Database;

namespace Planner.Application.Models;

public class Goal : BaseModel
{
    public string Name { get; set; } = "NoName";
    public virtual Contractor Contractor { get; set; } = new();
    public TimeSpan ElapsedTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public ICollection<GoalElapsedTimePart> ElapsedTimeParts { get; set; } = new HashSet<GoalElapsedTimePart>();

    public virtual User User { get; set; } = new();

    public void CollapseElapsedTime()
    {
        ElapsedTime = ElapsedTimeParts.Select(x => x.ElapsedTime).Aggregate((l, r) => l + r);
    }
}