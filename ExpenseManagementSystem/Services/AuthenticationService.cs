﻿using ExpenseManagementSystem.Models;
using ExpenseManagementSystem.Managers;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.Managers.Helper;
using ExpenseManagementSystem.DTOs.Authentication;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Services.Interfaces;

namespace ExpenseManagementSystem.Services;

public class AuthenticationService(IGenericRepository genericRepository, IserializerAndDeserializerManager serializerAndDeserializerManager, IlocalStorage localStorage) : IAuthServices 
{
    /// <summary>
    /// Retrieves users list from the respective user load file path
    /// </summary>
    /// <returns> List must have at least one record. Therefore, it returns whether a user is registered or not. </returns>
    public bool IsUserRegistered()
    {
        var users = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath);
        
        return users.Count != 0;
    }

    public async Task LoginUser(LoginRequestDto login)
    {
        var users = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath);

        var user = users.FirstOrDefault(x => x.Username == login.Username.Trim().ToLower());

        if (user == null)
        {
            throw new Exception("Invalid username, please try again.");
        }

        var isPasswordValid = login.Password.VerifyHash(user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Please provide a valid password.");
        }

        var result = new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Currency = user.Currency,
            Username = user.Username,
        };

        var userDetails = new List<UserDetailsDto>()
        {
            result
        };

        var serializedUserDetails = serializerAndDeserializerManager.Serialize(userDetails);

        await localStorage.SetItemAsync("user_details", serializedUserDetails);
    }

    public void RegisterNewUSer(RegisterRequestDto register)
    {
        register.Username = register.Username.Trim();

        if (register.Username == "" || register.Currency == ""|| register.Password == "")
        {
            throw new Exception("Please insert correct and valid input for each of the fields.");
        }

        var users = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath);

        var usernameExists = users.Any(x => x.Username == register.Username);

        if (usernameExists)
        {
            throw new Exception("Username already exists. Please choose any other username.");
        }

        var user = new User()
        {
            Username = register.Username,
            PasswordHash = register.Password.HashSecret(),
            Currency = register.Currency,
            CreatedAt = DateTime.Now,
            IsActive = true,
            Name = register.Username
        };

        users.Add(user);

        genericRepository.SaveAll(users, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppUsersDirectoryPath);
    }

    public void PerformLogout()
    {
        localStorage.ClearItemAsync("user_details");
    }
}
