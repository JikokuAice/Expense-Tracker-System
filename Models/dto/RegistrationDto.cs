using ExpenseTrackerSystem.Models.Enums;

namespace ExpenseTrackerSystem.Models.dto;

public class RegistrationDto
{
   public string Name { get; set; }
   public string Email { get; set; }
   public string Password { get; set; }
   public DateTime CreationDate {get; set;}
}