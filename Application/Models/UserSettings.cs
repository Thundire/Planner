using Spark.Library.Database;

namespace Planner.Application.Models;

public class UserSettings : BaseModel
{
	private const string DefaultTimeFormatter = @"hh\:mm";
	private const string DefaultDetailedTimeFormatter = @"hh\:mm\:ss";

	public User? User { get; set; }
	public string TimeFormatter { get; set; } = DefaultTimeFormatter;
	public string DetailedTimeFormatter { get; set; } = DefaultDetailedTimeFormatter;

	public static string TimeFormatterValue(UserSettings? instance) => instance?.TimeFormatter ?? DefaultTimeFormatter;
	public static string DetailedTimeFormatterValue(UserSettings? instance) => instance?.DetailedTimeFormatter ?? DefaultDetailedTimeFormatter;
}