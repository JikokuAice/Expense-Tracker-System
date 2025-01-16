namespace ExpenseManagementSystem.DTOs.Dashboard.Filters;

public class FilterForVisualization
{
      public bool IsAscending { get; set; } = true;
      public int Count { get; set; } = 5;
      public bool IsDisplayedAsBarChart { get; set; }
}