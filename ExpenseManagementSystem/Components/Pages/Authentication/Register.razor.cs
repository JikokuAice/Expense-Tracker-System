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
            if (Registration.Currency.ToUpper() != "USD" || Registration.Currency.ToUpper() != "EUR" || Registration.Currency.ToUpper() != "NPR")
            {
                throw new Exception("USD, EUR, NPR Currency is only supported.");
            }
            else
            {
                Registration.Currency=Registration.Currency.ToUpper();
            }
            
            AuthenticationService.RegisterNewUSer(Registration);
            
            SnackbarService.PopSnackBar("User successfully registered.", Severity.Success, Variant.Outlined);
            
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Info, Variant.Outlined);
        }
    }
    #endregion
}