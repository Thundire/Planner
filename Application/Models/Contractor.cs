using System.Diagnostics.Contracts;
using Spark.Library.Database;

namespace Planner.Application.Models;

public class Contractor : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public static Contractor Empty { get; } = new() { Name = "Empty" };

	public override bool Equals(object? obj) => obj is Contractor contractor && Name == contractor.Name;
	public override int GetHashCode() => HashCode.Combine(Name);
	public override string ToString() => Name;
}