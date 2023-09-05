using Coravel.Events.Interfaces;

namespace Planner.Application.Events
{
    public class GoalElapsedPartRemoved : IEvent
    {
        public int Id { get; init; }
        public int GoalId { get; set; }
        public int UserId { get; init; }
    }
}