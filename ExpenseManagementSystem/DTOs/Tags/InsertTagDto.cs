using MudBlazor.Utilities;
using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.DTOs.Tags;

public class InsertTagDto
{
    public string Name { get; set; }

    public MudColor BackgroundColor { get; set; } = new();

    public MudColor TextColor { get; set; } = new();
}
