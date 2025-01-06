using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.DTOs.Debts;

public class GetDebtDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public decimal Amount   { get; set; }   

    public string Source { get; set; }

    public DebtStatus Status { get; set; }

    public string DueDate { get; set; }

    public string? ClearedDate { get; set; }
}
