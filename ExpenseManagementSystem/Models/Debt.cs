using ExpenseManagementSystem.Models.Base;
using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.Models;

public class Debt : BaseEntity<Guid>
{
    public string Title { get; set; }

    public string Source { get; set; }

    public decimal Amount { get; set; }

    public DateOnly DueDate { get; set; }

    public DateTime? ClearedDate { get; set; }

    public DebtStatus Status { get; set; }
}
