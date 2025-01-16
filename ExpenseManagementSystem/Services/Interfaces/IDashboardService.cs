using ExpenseManagementSystem.DTOs.Dashboard;
using ExpenseManagementSystem.DTOs.Dashboard.Filters;
using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.Filters.Debts;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface IDashboardService
{
   Task<DashboardDtoData> FetchDashboardData();
   Task<List<DetailsForVisualization>> GetInflowsTransactions(FilterForVisualization transactionFilterRequest);
   Task<List<DetailsForVisualization>> GetOutflowsTransactions(FilterForVisualization transactionFilterRequest);
   Task<List<DetailsForVisualization>> GetDebtTransactions(FilterForVisualization transactionFilterRequest);
   Task<List<GetDebtDto>> GetDebtAsync(GetDebtFilterRequestDto debtFilterRequestDto);
   
   Task<string> FetchUserCurrencyName();
}