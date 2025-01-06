using MudBlazor;
using Microsoft.AspNetCore.Components;
using ExpenseManagementSystem.Components.Layout;
using ExpenseManagementSystem.DTOs.Authentication;

namespace ExpenseManagementSystem.Components.Pages.Authentication;

public partial class Register
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
    private RegisterRequestDto Registration { get; set; } = new();

    private void RegisterHandler()
    {
        try
        {
            AuthenticationService.RegisterNewUSer(Registration);
            
            SnackbarService.PopSnackBar("User successfully registered.", Severity.Success, Variant.Outlined);
            
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}