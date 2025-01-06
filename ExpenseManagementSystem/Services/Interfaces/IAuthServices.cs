using ExpenseManagementSystem.DTOs.Authentication;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface IAuthServices
{
    bool IsUserRegistered();

    Task LoginUser(LoginRequestDto login); 
    
    void RegisterNewUSer(RegisterRequestDto register);
    
    void PerformLogout(); 
}
