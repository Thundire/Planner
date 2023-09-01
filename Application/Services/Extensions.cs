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
	public static void Copy(this Goal target, Goal source, bool ignoreNavigationProperties = false, bool ignoreCollections = false)
	{
		target.Id           = source.Id;
		target.Name         = source.Name;
		target.CreatedAt    = source.CreatedAt;
		target.UpdatedAt    = source.UpdatedAt;
		target.Comment      = source.Comment;

		if (ignoreNavigationProperties) return;

		target.Contractor   = source.Contractor;
		target.User         = source.User;

		if(ignoreCollections) return;

		target.ElapsedTimeParts.Clear();
		target.ElapsedTimeParts.AddRange(source.ElapsedTimeParts);
	}

	public static void Copy(this ActiveGoal target, Goal source)
	{
		target.Id         = source.Id;
		target.Name       = source.Name;
		target.CreatedAt  = source.CreatedAt;
		target.UpdatedAt  = source.UpdatedAt;
		target.Comment    = source.Comment;
		target.Contractor = source.Contractor;
	}
}
