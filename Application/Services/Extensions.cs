using Planner.Application.Models;

namespace Planner.Application.Services;

public static class Extensions
{
	public static void Copy(this Contractor target, Contractor source)
	{
		target.Id = source.Id;
		target.Name = source.Name;
		target.CreatedAt = source.CreatedAt;
		target.UpdatedAt = source.UpdatedAt;
	}
}
