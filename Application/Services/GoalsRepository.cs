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

    public async Task<List<Goal>> List(User user)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    IQueryable<Goal> set = context.Goals.Where(x=>x.User.Id == user.Id).AsNoTracking();
	    return await set.ToListAsync();
	}
    public async Task<Goal> StoreOrUpdate(Goal data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var existed = await context.Goals.FindAsync(data.Id);
	    if (existed is null)
	    {
		    existed = new();
		    existed.Copy(data, true);
			
		    context.Contractors.Attach(data.Contractor);
		    context.Users.Attach(data.User);

			existed.Contractor = data.Contractor;
			existed.User = data.User;

		    context.Update(existed);
	    }
	    else
	    {
		    data.Copy(existed, true);
		    context.Contractors.Attach(data.Contractor);
		    existed.Contractor = data.Contractor;

		    var leftOuterJoin =
			    from outer in existed.ElapsedTimeParts
			    join inner in data.ElapsedTimeParts on outer.Id equals inner.Id into temp
			    from inner in temp.DefaultIfEmpty()
			    select new
			    {
				    Outer    = outer,
				    Inner    = inner,
				    ToDelete = inner is null,
				    ToIgnore = inner is not null,
				    ToAdd    = false,
			    };
		    var rightOuterJoin =
			    from inner in data.ElapsedTimeParts
				join outer in existed.ElapsedTimeParts on inner.Id equals outer.Id into temp
			    from outer in temp.DefaultIfEmpty()
			    select new
			    {
				    Outer    = outer,
				    Inner    = inner,
				    ToDelete = false,
				    ToIgnore = outer is not null,
				    ToAdd    = outer is null,
			    };
		    var join = leftOuterJoin.Union(rightOuterJoin).ToArray();
		    foreach (var result in join)
		    {
			    switch (result.ToAdd, result.ToDelete)
			    {
				    case (true, false):
					    context.GoalElapsedTimeParts.Attach(result.Inner);
					    existed.ElapsedTimeParts.Add(result.Inner);
					    break;
				    case (false, true):
					    existed.ElapsedTimeParts.Remove(result.Outer);
					    break;
			    }
		    }

			existed.CollapseElapsedTime();
	    }

	    await context.SaveChangesAsync();
	    return existed;
    }

    public async Task Destroy(Goal data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    context.Attach(data);
	    context.Remove(data);
	    await context.SaveChangesAsync();
    }

    public async Task<GoalElapsedTimePart> StoreOrUpdate(GoalElapsedTimePart data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		context.Update(data);
		await context.SaveChangesAsync();
		return data;
	}

    public async Task Remove(GoalElapsedTimePart data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    context.Attach(data);
	    context.Remove(data);
	    await context.SaveChangesAsync();
	}

    public async Task<Goal> CollapseTime(Goal data)
    {
	    await using DatabaseContext context = await _factory.CreateDbContextAsync();
	    var existed = await context.Goals.FindAsync(data.Id);
	    if (existed is null) throw new InvalidOperationException("Not found goal");
		existed.CollapseElapsedTime(true);
		await context.SaveChangesAsync();
		return existed;
    }
}