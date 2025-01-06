using ExpenseManagementSystem.Models.Base;

namespace ExpenseManagementSystem.Models;

public class TransactionTags : BaseEntity<Guid>    
{
    public Guid TransactionId { get; set; }

    public Guid TagId { get; set; }
}
