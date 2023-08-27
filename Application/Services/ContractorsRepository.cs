using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;
using Planner.Application.Models;

namespace Planner.Application.Services;

public class ContractorsRepository
{
    private readonly IDbContextFactory<DatabaseContext> _factory;

    public ContractorsRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Contractor>> List()
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Contractors.OrderBy(x=>x.Name).ToListAsync();
    }

    public async Task<Contractor> StoreOrUpdate(Contractor data)
    {
        await using var context = await _factory.CreateDbContextAsync();
        if (data.Id == 0)
        {
	        var existed = await context.Contractors.FirstOrDefaultAsync(x => x.Name == data.Name);
	        if (existed is not null) throw new InvalidOperationException("Trying store existed contractor");
		}
        context.Update(data);
        await context.SaveChangesAsync();
        return data;
    }

    public async Task Destroy(Contractor data)
    {
        await using var context = await _factory.CreateDbContextAsync();
        context.Remove(data);
        await context.SaveChangesAsync();
    }
}