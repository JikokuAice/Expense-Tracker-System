using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.Filters.Debts;

public class GetDebtFilterRequestDto : GetFilterRequestDto
{
    public DebtStatus? Status { get; set; }
    
    public DateTime? StartDate { get; set; } 

    public DateTime? EndDate { get; set; } 
}