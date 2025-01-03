using ExpenseTrackerSystem.Models.Enums;
using SQLite;

namespace ExpenseTrackerSystem.Models;

public class UserCurrency
{
   [PrimaryKey] [AutoIncrement] 
   public string Id { get; set; }
   public Currency ChoosenCurrency { get; set; }
}