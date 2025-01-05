using MudBlazor;
using Microsoft.AspNetCore.Components;
using ExpenseManagementSystem.DTOs.Tags;
using ExpenseManagementSystem.Filters.Tags;
using ExpenseManagementSystem.Components.Layout;

namespace ExpenseManagementSystem.Components.Pages.Tags;

public partial class Tags
{
    protected override async Task OnInitializedAsync()
    {
        SetPageTitle();
        
        await GetAllTags();
    }

    
    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Tags";
    }
    
    
   
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

        await GetAllTags();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetTagDto.Name);
    
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

        await GetAllTags();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }
 

    private List<GetTagDto> TagModels { get; set; } = new();

    private async Task GetAllTags()
    {
        var filterRequest = new GetTagFilterRequestDto
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
        };

        TagModels = await TagService.GetAllTags(filterRequest);
        
        StateHasChanged();
    }
 


    private bool IsInsertTagModalOpen { get; set; }

    private InsertTagDto TagModel { get; set; } = new();

    private void OpenCloseInsertTagModal()
    {
        IsInsertTagModalOpen = !IsInsertTagModalOpen;

        TagModel = new InsertTagDto();

        StateHasChanged();
    }

    private async Task InsertTag()
    {
        try
        {
            await TagService.InsertTag(TagModel);

            OpenCloseInsertTagModal();

            await GetAllTags();
            
            SnackbarService.PopSnackBar("Tag successfully created.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }

}