namespace ExpenseManagementSystem.DTOs.Transaction;

public class UpdateTransactionDto: InsertIntoTransactionDto
{
    public Guid Id { get; set; }
}
