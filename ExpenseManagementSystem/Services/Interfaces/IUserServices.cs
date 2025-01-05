using ExpenseManagementSystem.DTOs.Authentication;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface IUserServices
{
    Task<UserDetailsDto?> FetchUserDetail();

    List<UserDetailsDto> FetchTotalUsers();
}
