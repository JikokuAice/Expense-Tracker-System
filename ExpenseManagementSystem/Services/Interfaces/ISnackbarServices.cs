using MudBlazor;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface ISnackBarServices
{
    void PopSnackBar(string message, Severity severity, Variant variant);
}