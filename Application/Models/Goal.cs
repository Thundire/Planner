using Spark.Library.Database;

namespace Planner.Application.Models;

public class Goal : BaseModel
{
    public string Name { get; set; } = "NoName";
    public virtual Contractor Contractor { get; set; } = new();
    public TimeSpan ElapsedTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<GoalElapsedTimePart> ElapsedTimeParts { get; set; } = new ();

    public virtual User User { get; set; } = new();

    public void CollapseElapsedTime(bool fixCollapsed = false)
    {
        ElapsedTime = ElapsedTimeParts
	        .Select(x =>
            {
	            if (fixCollapsed) x.Collapsed = true;
	            return x.ElapsedTime;
            })
	        .Aggregate((l, r) => l + r);
    }
}