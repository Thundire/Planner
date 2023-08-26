using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;

namespace Planner.Application.Services;

public class GoalsRepository
{
    private readonly IDbContextFactory<DatabaseContext> _factory;

    public GoalsRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _factory = factory;
    }

}