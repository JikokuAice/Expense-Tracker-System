using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.DTOs.Transaction;

public class InsertIntoTransactionDto
{
    public string Title { get; set; }

    public string Note { get; set; }

    public TransactionType Type { get; set; } = TransactionType.None;

    public TransactionSource Source { get; set; } = TransactionSource.None;

    public decimal Amount { get; set; }

    public List<Guid> TagIds { get; set; } = [];
}

