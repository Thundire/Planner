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
	public static void Copy(this Goal target, Goal source, bool ignoreNavigationProperties = false)
	{
		target.Id          = source.Id;
		target.Name        = source.Name;
		target.CreatedAt   = source.CreatedAt;
		target.UpdatedAt   = source.UpdatedAt;
		target.ElapsedTime = source.ElapsedTime;
		target.Comment     = source.Comment;

		if(!ignoreNavigationProperties) return;

		target.Contractor = source.Contractor;
		target.User = source.User;
		target.ElapsedTimeParts.Clear();
		target.ElapsedTimeParts.AddRange(source.ElapsedTimeParts);
	}
}
