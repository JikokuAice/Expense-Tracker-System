using ExpenseManagementSystem.Models.Constant;

namespace ExpenseManagementSystem.DTOs.Authentication;

public class UserDetailsDto
{
    public Guid Id { get; set; }

    public string Name {  get; set; }

    public string Username { get; set; }

    public string Currency { get; set; }
}
