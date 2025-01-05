using MudBlazor;
using Microsoft.AspNetCore.Components;
using ExpenseManagementSystem.Components.Layout;
using ExpenseManagementSystem.DTOs.Authentication;

namespace ExpenseManagementSystem.Components.Pages.Authentication;

public partial class Login
{
    protected override void OnInitialized()
    {
        SetPageTitle();
    }

    #region Page Title
    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Login";
    }
    #endregion
    
    #region User Registration
    private LoginRequestDto LoginDetails { get; set; } = new();

    private async Task LoginHandler()
    {
        try
        {
            await AuthenticationService.LoginUser(LoginDetails);
            
            SnackbarService.PopSnackBar("User successfully logged in.", Severity.Success, Variant.Outlined);

            NavigationManager.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}