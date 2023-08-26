using Microsoft.EntityFrameworkCore;
using Planner.Application.Database;

namespace Planner.Application.Services;

public class ContractorsRepository
{
    private readonly IDbContextFactory<DatabaseContext> _factory;

    public ContractorsRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _factory = factory;
    }

}