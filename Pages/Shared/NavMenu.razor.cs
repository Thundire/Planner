using Microsoft.AspNetCore.Components;
using Planner.Application.Models;

namespace Planner.Pages.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout?.User;
}
