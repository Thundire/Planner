using Coravel.Events.Interfaces;

namespace Planner.Application.Events
{
    public class TimePartEditedByHand : IEvent
    {
	    public int UserId { get; set; }

		public int PartId { get; set; }
		public TimeSpan Time { get; set; }
		public string Comment { get; set; } = string.Empty;
		public DateTime UpdatedAt { get; set; }
	}
}