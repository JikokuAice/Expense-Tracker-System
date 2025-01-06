using MudBlazor;
using ExpenseManagementSystem.DTOs.Authentication;

namespace ExpenseManagementSystem.Components.Layout;

public partial class MainLayout
{
    public string PageTitle { get; set; } = "Personal Expense Tracker";
    
    private UserDetailsDto? UserDetails { get; set; }
    
    private bool DrawerOpen { get; set; } = true;
    
    private MudTheme Theme { get; } = new ()
    {
        ZIndex = new ZIndex
        {
            Drawer = 1300
        }
    };

    private static bool RightToLeft => false;

    protected override async Task OnInitializedAsync()
    {
        SeedData();
        
        await FetchUserDetail();
    }

    private async Task FetchUserDetail()
    {
        var userDetails = await UserService.FetchUserDetail();

        if (userDetails != null)
        {
            UserDetails = userDetails;
        }
        else
        {
            var users = UserService.FetchTotalUsers();
        
            NavigationManager.NavigateTo(users.Count > 0 ? "/login" : "/register");
        }
    }

    private void SeedData()
    {
        SeedingService.SeedDefaultTags();
    }
    
    private void LogoutHandler()
    {
        AuthenticationService.PerformLogout();

        SnackbarService.PopSnackBar("User successfully logged out.", Severity.Success, Variant.Outlined);
        
        NavigationManager.NavigateTo("/login", true);
    }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}