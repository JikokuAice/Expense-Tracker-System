using ExpenseManagementSystem.DTOs.Dashboard;
using ExpenseManagementSystem.DTOs.Dashboard.Filters;
using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.Filters.Debts;
using ExpenseManagementSystem.Services;
using ExpenseManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ExpenseManagementSystem.Components.Pages.Dashboard;

public partial class Dashboard : ComponentBase
{



   
   protected override async Task OnInitializedAsync()
   { 
      await GetDashboardData();
      await GetUsercCurrencyname();
     await GetAllGetDebtDtoList();
    await GetInflowsVisualizationDetails();
    await GetOutflowVisualizationDetails();
    await GetDebtVisualizationDetails();
    
   }

   
  private string UserCurrency  { get; set; }

  private async Task GetUsercCurrencyname()
  {
     try
     {
        UserCurrency = await DashboardService.FetchUserCurrencyName();
     }
     catch (Exception e)
     {
        Console.WriteLine(e);
        throw;
     }
  }
   
   
   private DashboardDtoData DashboardCardsData { get; set; } = new();

   private async Task GetDashboardData()
   {
      try
      {
         DashboardCardsData = await DashboardService.FetchDashboardData();
      }
      catch (Exception e)
      {
        SnackbarService.PopSnackBar(e.Message, Severity.Error, Variant.Outlined); 
      }
      
   }
   
   
   private void OnDebtFilterHandler(bool obj)
   {
      throw new NotImplementedException();
   }
   
   private DateTime? StartDate { get; set; }

   private DateTime? EndDate { get; set; }


   private string _search = string.Empty;
   
   
   public string Search {get => _search;
      set
      {
         if(_search == value) return;
          _search = value;
          _ = onSearchInput(_search);
      }
   
   
   }

   
   private string CurrentSortColumn { get; set; } = nameof(GetDebtDto.Title);
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

      await GetAllGetDebtDtoList();
   }

   private string GetSortIcon(string column)
   {
      if (CurrentSortColumn != column)
      {
         return Icons.Material.Filled.UnfoldMore;
      }

      return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
   }

  
   
  private async Task  onSearchInput(string search)
   {
      Search = search;

   await  GetAllGetDebtDtoList();
   }
  
  

   
  
  
  private GetDebtDto DebtDtoModel  = new GetDebtDto();

  private List<GetDebtDto> PendingDebtList { get; set; } = new List<GetDebtDto>();

   private async Task GetAllGetDebtDtoList()
   {
      try
      {
         GetDebtFilterRequestDto filterRequest = new()
         { 
            Search = Search,
            StartDate = StartDate,
            EndDate = EndDate,
            IsDescending = IsSortDescending,
            OrderBy = CurrentSortColumn
            
         }; 
         
         
        PendingDebtList = await DashboardService.GetDebtAsync(filterRequest);
         
         
      }
      catch (Exception ex)
      {
         SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
      }
      
   }
   
   
   
   
   private FilterForVisualization InflowsVisualizationFilter { get; set; } = new();
   private List<DetailsForVisualization> InflowDataListForVisualization { get; set; } = [];
 


   public async Task GetInflowsVisualizationDetails()
   {
      try
      {
         InflowDataListForVisualization = await DashboardService.GetInflowsTransactions(InflowsVisualizationFilter);
            
         StateHasChanged();
      }
      catch (Exception ex)
      {
         SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
      }

   }
   
   
   private FilterForVisualization OutflowsVisualizationFilter { get; set; } = new();
   private List<DetailsForVisualization> OutFlowDataListForVisualization { get; set; } = [];
   
   public async Task GetOutflowVisualizationDetails()
   {
      try
      {
         OutFlowDataListForVisualization = await DashboardService.GetOutflowsTransactions(OutflowsVisualizationFilter);
            
         StateHasChanged();
      }
      catch (Exception ex)
      {
         SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
      }

   }
   
   
   private FilterForVisualization DebtVisualizationFilter { get; set; } = new();
   private List<DetailsForVisualization> DebtDataListForVisualization { get; set; } = [];
   
   public async Task GetDebtVisualizationDetails()
   {
      try
      {
       DebtDataListForVisualization = await DashboardService.GetDebtTransactions(DebtVisualizationFilter);
            
         StateHasChanged();
      }
      catch (Exception ex)
      {
         SnackbarService.PopSnackBar(ex.Message, Severity.Error, Variant.Outlined);
      }

   }
   
   
   
   
}