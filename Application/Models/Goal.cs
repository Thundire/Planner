using System.ComponentModel.DataAnnotations.Schema;
using Spark.Library.Database;

namespace Planner.Application.Models;

public class Goal : BaseModel
{
    public string Name { get; set; } = "NoName";
    public TimeSpan ElapsedTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    [NotMapped]
    public bool ShowParts { get; set; }
    [NotMapped]
    public bool ShowCollapsedParts { get; set; }

    public List<GoalElapsedTimePart> ElapsedTimeParts { get; set; } = new();
	public int ContractorId { get; set; }
    public Contractor? Contractor { get; set; } = new();
	public int UserId { get; set; }
    public User? User { get; set; } = new();

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