using ExpenseManagementSystem.Managers;
using ExpenseManagementSystem.Models;
using ExpenseManagementSystem.Managers;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.DTOs.Authentication;
using ExpenseManagementSystem.Services.Interfaces;

namespace ExpenseManagementSystem.Services;

public class UserService(IserializerAndDeserializerManager serializerAndDeserializerManager, IlocalStorage localStorage, IGenericRepository genericRepository) : IUserServices
{
    public async Task<UserDetailsDto?> FetchUserDetail()
    {
        var userDetails = await localStorage.GetItemAsync<string>("user_details");

        if (userDetails == null)
        {
            return null;
        }

        var deserializedUserDetails = serializerAndDeserializerManager.Deserialize<UserDetailsDto>(userDetails);

        if (deserializedUserDetails.Count == 0)
        {
            return null;
        }

        var result = deserializedUserDetails.FirstOrDefault();

        var user = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath).FirstOrDefault(x => x.Id == result?.Id);

        if (user == null) return null;
        
        return new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Currency = user.Currency,
            Username = user.Username
        };
    }

    public List<UserDetailsDto> FetchTotalUsers()
    {
        var users = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath);

        return users.Select(x => new UserDetailsDto()
        {
            Id = x.Id,
            Name = x.Name,
            Username = x.Username,
            Currency = x.Currency
        }).ToList();
    }
}
