using Microsoft.AspNetCore.Components;
using MudBlazor;
using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.DTOs.Transaction;
using ExpenseManagementSystem.Filters.Debts;
using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.Components.Pages.Debts;

public partial class DebtDetails
{
    [Parameter] 
    public DebtStatus? DebtStatus { get; set; }

    [Parameter] 
    public EventCallback OnDebtsCountUpdate { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await GetAllDebts();

        await GetBalanceAndDebtDetails();
    }
    
    #region Search with Filter and Order
    private string _search = string.Empty;

    private string Search
    {
        get => _search;
        set
        {
            if (_search == value) return;
            _search = value;
            _ = OnSearchInputAsync(_search);
        }
    }

    private async Task OnSearchInputAsync(string search)
    {
        Search = search;

        await GetAllDebts();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetTransactionDto.Title);

    private bool IsSortDescending { get; set; }

    private async Task ChangeSorting(string column)
    {
        if (CurrentSortColumn == column)
        {
            IsSortDescending = !IsSortDescending;
        }
        else
        {
            CurrentSortColumn = column;
            IsSortDescending = false;
        }

        await GetAllDebts();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }

    private DateTime? StartDate { get; set; }

    private DateTime? EndDate { get; set; }

    private async Task OnDebtFilterHandler(bool isFilterApplied)
    {
        if (!isFilterApplied)
        {
            StartDate = null;
            EndDate = null;
        }
        
        await GetAllDebts();
    }
    #endregion

    #region Balance
    private decimal Balance { get; set; }

    private decimal PendingDebtAmount { get; set; }

    private async Task GetBalanceAndDebtDetails()
    {
        Balance = await DebtService.GetUserReminingBalance();

        PendingDebtAmount = await DebtService.FtechPendingDebtTotalAmt();
    }
    #endregion

    #region Debts Details
    private List<GetDebtDto> DebtDtoModels { get; set; } = new();

    private async Task GetAllDebts()
    {
        var filterRequest = new GetDebtFilterRequestDto()
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
            StartDate = StartDate,
            EndDate = EndDate,
            Status = DebtStatus
        };

        DebtDtoModels = await DebtService.GetAllDebts(filterRequest);

        StateHasChanged();
    }
    #endregion

    #region Debts Creation
    private bool IsInsertUserDebtModalOpen { get; set; }
    
    private InsertUserDebtDto InsertUserDebtDtoModel  { get; set; } = new();
    
    private void OpenCloseInsertUserDebtModal()
    {
        IsInsertUserDebtModalOpen = !IsInsertUserDebtModalOpen;

        InsertUserDebtDtoModel  = new InsertUserDebtDto();
        
        StateHasChanged();
    }

    private async Task InsertUserDebt()
    {
        try
        {
            await DebtService.InsertUserDebt(InsertUserDebtDtoModel );

            await OnDebtsCountUpdate.InvokeAsync();
            
            await GetBalanceAndDebtDetails();
            
            OpenCloseInsertUserDebtModal();
            
            await GetAllDebts();
            
            SnackbarService.PopSnackBar("Debt successfully created.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion

    #region Clear Debt
    private GetDebtDto GetDebtDtoModel  { get; set; } = new();

    private bool IsClearUserDebtModalOpen { get; set; }

    private void OpenCloseClearUserDebtModal(Guid debtId)
    {
        IsClearUserDebtModalOpen = !IsClearUserDebtModalOpen;

        GetDebtDtoModel  = IsClearUserDebtModalOpen ? DebtService.FetchDebtById(debtId) : new();

        StateHasChanged();
    }

    private async Task ClearUserDebt()
    {
        try
        {
            await DebtService.ClearUserDebt(GetDebtDtoModel .Id);

            OpenCloseClearUserDebtModal(GetDebtDtoModel .Id);

            await OnDebtsCountUpdate.InvokeAsync();
            
            await GetBalanceAndDebtDetails();

            await GetAllDebts();
            
            SnackbarService.PopSnackBar("Debt successfully cleared.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}