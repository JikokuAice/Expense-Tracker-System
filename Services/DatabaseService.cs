using ExpenseTrackerSystem.Models;
using ExpenseTrackerSystem.Models.dto;
using SQLite;

namespace ExpenseTrackerSystem.Services;

public class DatabaseService{
   
   SQLiteAsyncConnection _database;

   public DatabaseService()
   {
      
   }

   async Task Init()
   {
      if (_database is not null)
      {
         return;
      }
      _database = new SQLiteAsyncConnection(UtilityServices.DatabasePath,UtilityServices.Flags);
      await _database.CreateTableAsync<User>();
   }
   
   public async  Task<bool> AddUserAsync(User user)
   {
      await Init();
      var checkUserExist =await _database.FindAsync<User>(e=>e.Email == user.Email);
      if (checkUserExist is not null)
      {
         return false;
      }
      await _database.InsertAsync(user);
      return true;

   }


   public async Task<User?> AuthenticateAsync(LoginDto loginDto)
   {
      await Init();
      User authUser = await _database.FindAsync<User>(e => e.Email == loginDto.email && e.Password == loginDto.password);
      
      if (authUser == null)
      {
         return null;
      }
      return authUser;
   }
   
   
}