using MudBlazor;
using ExpenseManagementSystem.Services.Interfaces;

namespace ExpenseManagementSystem.Services;

public class SnackbarService(ISnackbar snackbar) : ISnackBarServices
{
    public void PopSnackBar(string message, Severity severity, Variant variant)
    {
        snackbar.Add(message, severity, c => c.SnackbarVariant = variant);
    }
}