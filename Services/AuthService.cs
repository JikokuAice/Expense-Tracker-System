using ExpenseTrackerSystem.Models;
using ExpenseTrackerSystem.Models.dto;

namespace ExpenseTrackerSystem.Services;

public class AuthService
{
   private DatabaseService _db; 
   public AuthService()
   {
      _db = new DatabaseService();
   }
   
   public async Task<bool> Register(RegistrationDto registrationDto)
   {
      var user = new User()
      {
         Name = registrationDto.Name,
         Email = registrationDto.Email,
         CreationDate = DateTime.Now,
         Password = registrationDto.Password,
      };
      
   var authenticated = await _db.AddUserAsync(user);
   return authenticated;   
   }
   
   public async Task<User?> Login(LoginDto loginDto)
   {
      var authenticated = await _db.AuthenticateAsync(loginDto);
         return authenticated;
         
   }
}