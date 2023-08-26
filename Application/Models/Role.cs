using Spark.Library.Database;

namespace Planner.Application.Models;

public class Role : BaseModel
{
	public string Name { get; set; } = string.Empty;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}
