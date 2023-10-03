using Planner.Application.Events.Listeners;
using Planner.Application.Events;
using Coravel.Events.Interfaces;
using Coravel;

namespace Planner.Application.Startup;

public static class Events
{
    public static IServiceProvider RegisterEvents(this IServiceProvider services)
    {
        IEventRegistration registration = services.ConfigureEvents();

        // add events and listeners here
        registration.Register<UserCreated>().Subscribe<EmailNewUser>();
        registration.Register<GoalChanged>().Subscribe<UpdateGoal>();
        registration.Register<GoalElapsedPartRemoved>().Subscribe<RemoveGoalElapsedPart>();
        registration.Register<JobBuildRequested>().Subscribe<BuildJob>();
        registration.Register<TimePartEditedByHand>().Subscribe<EditTimePartByHand>();

        return services;
    }
}
