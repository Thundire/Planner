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
	    IQueryable<Goal> set = context.Goals.Include(x=>x.ElapsedTimeParts).Where(x=> x.User != null && x.User.Id == user.Id).AsNoTracking();
	    return await set.ToListAsync();
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
		    //   join inner in data.ElapsedTimeParts on outer.Id equals inner.Id into temp
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
		    //   from inner in data.ElapsedTimeParts
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