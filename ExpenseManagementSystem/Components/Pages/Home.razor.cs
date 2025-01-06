namespace ExpenseManagementSystem.Components.Pages;

public partial class Home
{
    protected override async Task OnInitializedAsync()
    {
        var userDetails = await UserService.FetchUserDetail();

        if (userDetails != null)
        {
            NavigationManager.NavigateTo("/dashboard");
            
            return;
        }

        var users = UserService.FetchTotalUsers();
        
        NavigationManager.NavigateTo(users.Count > 0 ? "/login" : "/register");
    }
}