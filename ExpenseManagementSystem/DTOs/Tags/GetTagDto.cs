using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.DTOs.Tags;

public class GetTagDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TransactionType TransactionType { get; set; }

    public string BackgroundColor { get; set; }

    public string TextColor { get; set; }

    public bool IsDefault { get; set; }
}
