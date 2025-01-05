using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ExpenseManagementSystem.Components.Layout;

public partial class Filter
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
   
    [Parameter] public EventCallback<bool> OnFilterApplication { get; set; }

    private MudMenu FilterMenu { get; set; } = new();
    
    private async Task WhenFilterApplied(bool isFilterApplied)
    {
        if (isFilterApplied) await OnFilterApplication.InvokeAsync(true);
        
        await FilterMenu.OpenChanged.InvokeAsync(false);
        
        StateHasChanged();
    }
    
    private async Task WhenFilterCleared()
    {
        await OnFilterApplication.InvokeAsync(false);
        
        StateHasChanged();
    }
}