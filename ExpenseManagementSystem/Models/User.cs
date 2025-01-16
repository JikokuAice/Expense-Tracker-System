using ExpenseManagementSystem.Models.Base;
using System.Data;
using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.Models;

public class User : BaseEntity<Guid>
{
    public string Username { get; set; }

    public string Name { get; set; }

    public string PasswordHash { get; set; }

    public string Currency { get; set; }
}


