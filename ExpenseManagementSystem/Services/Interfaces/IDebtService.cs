using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.Filters.Debts;
using ExpenseManagementSystemr.DTOs.Debts;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface IDebtServices
{
    Task<decimal>GetUserReminingBalance();

    Task<decimal> FtechPendingDebtTotalAmt();

    Task<GetDebtsCountDto> GetDebtsCount();
    
    GetDebtDto FetchDebtById(Guid id);
    
    Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest);

    Task InsertUserDebt(InsertUserDebtDto debt);

    Task UpdateUserDebt(UpdateUserDebtDto transaction);

    Task ClearUserDebt(Guid debtId);

    Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
