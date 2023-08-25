using Microsoft.AspNetCore.Components;
using Planner.Application.Models;
using Planner.Pages.Shared;

namespace Planner.Pages;

public partial class Dashboard
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout.User;
}
