using Microsoft.EntityFrameworkCore;

using Planner.Application.Database;
using Planner.Application.Models;

using Spark.Library.Extensions;

namespace Planner.Application.Services;

public class UserSettingsRepository
{
	private readonly IDbContextFactory<DatabaseContext> _factory;

	public UserSettingsRepository(IDbContextFactory<DatabaseContext> factory)
	{
		_factory = factory;
	}

	public async Task<UserSettings> Settings(User user)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		User? existedUser = await context.Users.FindAsync(user.Id);
		if (existedUser is null) throw new InvalidOperationException("Get settings for not existed User");
		UserSettings? settings = await context.UserSettings.FirstOrDefaultAsync(x => x.User != null && x.User.Id == user.Id);
		if (settings is null)
		{
			settings = new() { User = existedUser };
			context.UserSettings.Save(settings);
		}
		return settings;
	}

	public async Task<bool> UpdateTimeFormatters(TimeFormattersChanged data)
	{
		await using DatabaseContext context = await _factory.CreateDbContextAsync();
		UserSettings? existed = await context.UserSettings.FirstOrDefaultAsync(x => x.User != null && x.User.Id == data.UserId);
		if (existed is not null)
		{
			existed.TimeFormatter         = data.TimeFormatter;
			existed.DetailedTimeFormatter = data.DetailedTimeFormatter;
			await context.SaveChangesAsync();
			return true;
		}

		return false;
	}
}