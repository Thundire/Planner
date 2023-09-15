using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;
using Planner.Application.Models;

namespace Planner.Application.Services;

public class JobsRepository
{
	private readonly IDbContextFactory<DatabaseContext> _factory;

	public JobsRepository(IDbContextFactory<DatabaseContext> factory)
	{
		_factory = factory;
	}

	public async Task<List<JobsNotes>> List(int userId)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		return await context.JobsNotes.AsNoTracking().Include(x => x.Notes).OrderByDescending(x=>x.CreatedAt).ToListAsync();
	}

	public async Task<JobsNotes?> One(int id)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		return await context.JobsNotes.AsNoTracking().Include(x => x.Notes).FirstOrDefaultAsync(x=>x.Id == id);
	}

	public async Task<JobsNotes> Build(int userId, DateTime requestedAt)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		User? user = await context.Users.FindAsync(userId);
		if (user is null) throw new InvalidOperationException("Trying to build job from user that not exist");
		var userGoals = await context.Goals.Include(x=>x.ElapsedTimeParts).Where(x=>x.User == user && x.ElapsedTimeParts.Count > 0).ToListAsync();
		
		JobsNotes notes = new ()
		{
			User = user,
			CreatedAt = requestedAt,
			Date = requestedAt
		};

		foreach (Goal userGoal in userGoals)
		{
			TimeSpan totalElapsedTime = userGoal.CollapseElapsedTime();
			userGoal.ElapsedTimeParts.Clear();

			notes.Notes.Add(new()
			{
				CreatedAt = requestedAt,
				Name = userGoal.Name,
				Comment = userGoal.Comment,
				Time = totalElapsedTime
			});
		}

		context.JobsNotes.Update(notes);
		await context.SaveChangesAsync();

		return notes;
	}
}