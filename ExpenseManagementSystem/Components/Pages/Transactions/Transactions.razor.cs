using Microsoft.AspNetCore.Components;
using ExpenseManagementSystem.Components.Layout;
using ExpenseManagementSystem.DTOs.Transaction;

namespace ExpenseManagementSystem.Components.Pages.Transactions;

public partial class Transactions
{
    protected override async Task OnInitializedAsync()
    {
        SetPageTitle();
        
        await TransactionCountGet();
    }

    #region Page Title
    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Transactions";
    }
    #endregion
    
    #region Tab Counts
    private int ActivePanelIndex { get; set; }
    private TransactionCountGetDto TransactionsCount { get; set; } = new();

    private async Task TransactionCountGet()
    {
        TransactionsCount = await TransactionService.TransactionCountGet();
    }
    #endregion
    
    #region Component Available Trainings Update on Count 
    private async Task HandleTransactionCounts()
    {
        await TransactionCountGet();
        
        StateHasChanged();
    }
    #endregion
}