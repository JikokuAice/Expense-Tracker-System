namespace ExpenseManagementSystem.DTOs.Debts;

public class InsertUserDebtDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public int Amount { get; set; }

    public string Source { get; set; }

    public DateTime? DueDate { get; set; }
}