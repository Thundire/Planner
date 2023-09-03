using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;
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
		    //  var leftOuterJoin =
		    //   from outer in existed.ElapsedTimeParts
		    //   join inner in startData.ElapsedTimeParts on outer.Id equals inner.Id into temp
		    //   from inner in temp.DefaultIfEmpty()
		    //   select new
		    //   {
		    //    Outer    = outer,
		    //    Inner    = inner,
		    //    ToDelete = inner is null,
		    //    ToIgnore = inner is not null,
		    //    ToAdd    = false,
		    //   };
		    //  var rightOuterJoin =
		    //   from inner in startData.ElapsedTimeParts
		    //join outer in existed.ElapsedTimeParts on inner.Id equals outer.Id into temp
		    //   from outer in temp.DefaultIfEmpty()
		    //   select new
		    //   {
		    //    Outer    = outer,
		    //    Inner    = inner,
		    //    ToDelete = false,
		    //    ToIgnore = outer is not null,
		    //    ToAdd    = outer is null,
		    //   };
		    //  var join = leftOuterJoin.Union(rightOuterJoin).ToArray();
		    //  foreach (var result in join)
		    //  {
		    //   switch (result.ToAdd, result.ToDelete)
		    //   {
		    //    case (true, false):
		    //	    context.GoalElapsedTimeParts.Attach(result.Inner);
		    //	    existed.ElapsedTimeParts.Add(result.Inner);
		    //	    break;
		    //    case (false, true):
		    //	    existed.ElapsedTimeParts.Remove(result.Outer);
		    //	    break;
		    //   }
		    //  }
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

    public async Task Remove(GoalElapsedTimePart data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    context.Attach(data);
	    context.Remove(data);
	    await context.SaveChangesAsync();
	}

    public async Task Update(ActiveGoal goal)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		var existed = await context.Goals.FindAsync(goal.Id);
		if (existed is null) throw new InvalidOperationException("Not found goal");
		existed.Name       = goal.Name;
		existed.Contractor = Equals(goal.Contractor, Contractor.Empty) ? null : goal.Contractor;
		existed.Comment    = goal.Comment;
		existed.UpdatedAt  = DateTime.UtcNow;
		await context.SaveChangesAsync();
	}
}

public record GoalElapsedTimePartStartData(int GoalId, DateTime StartTime);
public record GoalElapsedTimePartData(int PartId, TimeSpan ElapsedTime, DateTime UpdatedAt);