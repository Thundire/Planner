using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;
using Planner.Application.Events;
using Planner.Application.Models;

namespace Planner.Application.Services;

public class GoalsRepository
{
    private readonly IDbContextFactory<DatabaseContext> _factory;

    public GoalsRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<ActiveGoal>> List(User user)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    IQueryable<Goal> set = context.Goals.Include(x=>x.ElapsedTimeParts).Where(x=> x.User != null && x.User.Id == user.Id).AsNoTracking();
	    return await set.Select(x=>new ActiveGoal(x)).ToListAsync();
	}

    public async Task<ActiveGoal?> One(int id, int userId)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		Goal? goal = await context.Goals.AsNoTracking().Include(x => x.ElapsedTimeParts).FirstOrDefaultAsync(x => x.Id == id && x.User != null && x.User.Id == userId);
		return goal is null ? null : new ActiveGoal(goal);
	}
    public async Task<Goal?> OneFull(int id, int userId)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		Goal? goal = await context.Goals.AsNoTracking().Include(x => x.ElapsedTimeParts).FirstOrDefaultAsync(x => x.Id == id && x.User != null && x.User.Id == userId);
		return goal;
	}

    public async Task<Goal> StoreOrUpdate(Goal data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var existed = await context.Goals.FindAsync(data.Id);
	    if (existed is null)
	    {
		    existed = new();
		    existed.Copy(data, false);
		    var user = await context.Users.FindAsync(data.User!.Id);
		    existed.User = user;
	    }
	    else
	    {
		    existed.Copy(data, true);
	    }

	    if (!Equals(existed.Contractor, data.Contractor))
	    {
		    if(data.Contractor is not null) context.Attach(data.Contractor);
		    existed.Contractor = data.Contractor;
	    }
	    context.Update(existed);

		await context.SaveChangesAsync();
	    return existed;
    }

    public async Task<ActiveGoal> Store(string name, int userId, DateTime createdAt)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var user = await context.Users.FindAsync(userId);
		if (user is null) throw new InvalidOperationException("User not exist");
		Goal goal = new()
		{
			CreatedAt = createdAt,
			Name      = name,
			User      = user
		};
		context.Goals.Add(goal);
		await context.SaveChangesAsync();
		return new(goal);
	}

    public async Task Destroy(int id)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var goal = await context.Goals.FindAsync(id);
	    if (goal is not null)
	    {
		    context.Goals.Remove(goal);
		    await context.SaveChangesAsync();
	    }
    }

    public async Task<GoalElapsedTimePart> Store(GoalElapsedTimePartStartData startData)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var goal = await context.Goals.FindAsync(startData.GoalId);
		if (goal is null) throw new InvalidOperationException("Goal not existed");
		GoalElapsedTimePart elapsedTimePart = new()
		{
			Goal = goal,
			CreatedAt = startData.StartTime
		};
		context.GoalElapsedTimeParts.Add(elapsedTimePart);
		await context.SaveChangesAsync();
		return elapsedTimePart;
	}

    public async Task<GoalElapsedTimePart> Update(GoalElapsedTimePartData data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var elapsedTimePart = await context.GoalElapsedTimeParts.Include(x=>x.Goal).FirstOrDefaultAsync(x=>x.Id == data.PartId);
		if (elapsedTimePart is null) throw new InvalidOperationException("ElapsedTimePart not existed");
		elapsedTimePart.ElapsedTime = data.ElapsedTime;
		elapsedTimePart.UpdatedAt   = data.UpdatedAt;
		await context.SaveChangesAsync();
		return elapsedTimePart;
	}

    public async Task Remove(int id)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var elapsedTimePart = await context.GoalElapsedTimeParts.FindAsync(id);
		if(elapsedTimePart is null) return;
		context.Remove(elapsedTimePart);
	    await context.SaveChangesAsync();
	}

    public async Task Update(GoalChanged data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var existed = await context.Goals.FindAsync(data.Id);
		if (existed is null) throw new InvalidOperationException("Not found goal");
		existed.Name       = data.Name;
		existed.Contractor = data.Contractor;
		existed.Comment    = data.Comment;
		existed.UpdatedAt  = data.UpdatedAt;
		await context.SaveChangesAsync();
	}

    public async Task<List<ActiveGoal>> Update(List<GoalElapsedTimePart> elapsedParts)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var set = await context.GoalElapsedTimeParts.Include(x => x.Goal).ToListAsync();
	    var join = from s in set
		    join e in elapsedParts on s.Id equals e.Id
		    select new {existed= s, received= e};
	    HashSet<int> affectedGoalsIds = new();
	    foreach (var data in join)
	    {
		    affectedGoalsIds.Add(data.existed.Goal.Id);
			data.existed.ElapsedTime = data.received.ElapsedTime;
			data.existed.UpdatedAt   = data.received.UpdatedAt;
		}
	    await context.SaveChangesAsync();
		
	    var affectedGoals = await context.Goals.AsNoTracking().Include(x => x.ElapsedTimeParts).Where(x => affectedGoalsIds.Contains(x.Id)).ToListAsync();
	    return affectedGoals.Select(x=> new ActiveGoal(x)).ToList();
	}
}

public record GoalElapsedTimePartStartData(int GoalId, DateTime StartTime);
public record GoalElapsedTimePartData(int PartId, TimeSpan ElapsedTime, DateTime UpdatedAt);