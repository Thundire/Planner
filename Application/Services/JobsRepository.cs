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
		return await context.JobsNotes.AsNoTracking().Include(x => x.Notes).ToListAsync();
	}

	public async Task<JobsNotes?> One(int id)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		return await context.JobsNotes.AsNoTracking().Include(x => x.Notes).FirstOrDefaultAsync(x=>x.Id == id);
	}

	public async Task<JobsNotes> Build(int userId)
	{
		return new();
	}
}