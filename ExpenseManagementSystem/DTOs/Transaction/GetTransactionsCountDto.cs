namespace ExpenseManagementSystem.DTOs.Transaction;

public class TransactionCountGetDto
{
    public int AllCount { get; set; }
    
    public int InflowsCount { get; set; }

    public int OutflowsCount { get; set; }
}
