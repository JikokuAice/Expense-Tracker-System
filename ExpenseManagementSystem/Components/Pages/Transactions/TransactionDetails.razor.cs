using MudBlazor;
using Microsoft.AspNetCore.Components;
using ExpenseManagementSystem.DTOs.Tags;
using ExpenseManagementSystem.Filters.Tags;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.DTOs.Transaction;
using ExpenseManagementSystem.Filters.Transactions;

namespace ExpenseManagementSystem.Components.Pages.Transactions;

public partial class TransactionDetails
{
    [Parameter] 
    public TransactionType? TransactionType { get; set; }

    [Parameter] public EventCallback OnTransactionsCountUpdate { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetAllTags();

        await GetBalanceDetails();
        
        await GetAllTransactions();
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

        await GetAllTransactions();
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

        await GetAllTransactions();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }

    private IEnumerable<Guid> FilterTagIdentifiers { get; set; } = [];
    
    private DateTime? StartDate { get; set; }

    private DateTime? EndDate { get; set; }

    private async Task OnTransactionFilterHandler(bool isFilterApplied)
    {
        if (!isFilterApplied)
        {
            FilterTagIdentifiers = [];
            StartDate = null;
            EndDate = null;
        }
        
        await GetAllTransactions();
    }
    #endregion

    #region Balance
    private decimal Balance { get; set; }

    private async Task GetBalanceDetails()
    {
        Balance = await TransactionService.GetUserReminingBalance();
    }
    #endregion

    #region Transaction Details
    private List<GetTransactionDto> TransactionModels { get; set; } = new();

    private async Task GetAllTransactions()
    {
        var filterRequest = new GetTransactionFilterRequestDto
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
            TagIds = FilterTagIdentifiers.ToList(),
            StartDate = StartDate,
            EndDate = EndDate,
            TransactionType = TransactionType
        };

        TransactionModels = await TransactionService.GetAllTransactions(filterRequest);

        StateHasChanged();
    }
    #endregion

    #region Transaction Creation
    private bool IsInsertIntoTransactionModalOpen { get; set; }
    
    private List<GetTagDto> Tags { get; set; } = [];

    private IEnumerable<Guid> TagIdentifiers { get; set; } = [];
    
    private InsertIntoTransactionDto InsertIntoTransactionModel { get; set; } = new();

    private async Task GetAllTags()
    {
        Tags = await TagService.GetAllTags(new GetTagFilterRequestDto());
    }
    
    private void OpenCloseInsertIntoTransactionModal()
    {
        IsInsertIntoTransactionModalOpen = !IsInsertIntoTransactionModalOpen;

        InsertIntoTransactionModel = new InsertIntoTransactionDto();

        TagIdentifiers = [];
        
        StateHasChanged();
    }

    private async Task InsertIntoTransaction()
    {
        try
        {
            InsertIntoTransactionModel.TagIds = TagIdentifiers.ToList();
            
            await TransactionService.InsertIntoTransaction(InsertIntoTransactionModel);

            await OnTransactionsCountUpdate.InvokeAsync();

            OpenCloseInsertIntoTransactionModal();

            await GetAllTransactions();

            await GetBalanceDetails();
            
            SnackbarService.PopSnackBar("Transaction successfully created.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion

    #region Transaction Note
    private UpdateTransactionDto UpdateTransactionModel { get; set; } = new();

    private bool IsUpdateTransactionNoteModalOpen { get; set; }

    private void OpenCloseUpdateTransactionNoteModal(Guid transactionId)
    {
        IsUpdateTransactionNoteModalOpen = !IsUpdateTransactionNoteModalOpen;

        var transactionDetails = TransactionService.GetTransactionById(transactionId);

        UpdateTransactionModel = new UpdateTransactionDto()
        {
            Id = transactionDetails.Id,
            Amount = transactionDetails.Amount,
            Note = transactionDetails.Note,
            Title = transactionDetails.Title,
            Source = transactionDetails.Source,
            Type = transactionDetails.Type,
            TagIds = transactionDetails.Tags.Select(x => x.Id).ToList()
        };
        
        StateHasChanged();
    }

    private async Task UpdateTransactionNote()
    {
        try
        {
            await TransactionService.UpdateTransaction(UpdateTransactionModel);

            OpenCloseUpdateTransactionNoteModal(UpdateTransactionModel.Id);
            
            await GetAllTransactions();
            
            SnackbarService.PopSnackBar("Transaction note successfully updated.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}