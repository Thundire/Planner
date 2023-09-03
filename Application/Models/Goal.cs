using System.ComponentModel.DataAnnotations.Schema;
using Spark.Library.Database;

namespace Planner.Application.Models;

public class Goal : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public List<GoalElapsedTimePart> ElapsedTimeParts { get; set; } = new();
    public Contractor? Contractor { get; set; }
    public User? User { get; set; }

    public TimeSpan CollapseElapsedTime()
    {
        if(ElapsedTimeParts.Count <= 0) return TimeSpan.Zero;
        return ElapsedTimeParts
	        .Select(x => x.ElapsedTime)
	        .Aggregate((l, r) => l + r);
    }
}