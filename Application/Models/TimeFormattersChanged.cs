namespace Planner.Application.Models;

public class TimeFormattersChanged
{
	public int UserId { get; set; }
	public string TimeFormatter { get; init; } = string.Empty;
	public string DetailedTimeFormatter { get; init; } = string.Empty;
}