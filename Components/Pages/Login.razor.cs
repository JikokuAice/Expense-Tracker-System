using ExpenseTrackerSystem.Models.dto;
using ExpenseTrackerSystem.Models.Enums;
using ExpenseTrackerSystem.Services;
using Microsoft.AspNetCore.Components;

namespace ExpenseTrackerSystem.Components.Pages;

public partial class Login : ComponentBase
{
   
   private string? email;
   private string? password;
   private Currency selectedCurrency;
   private string errorMessage = "";
   
   
   
   private void NavigateToRegister()
   { 
      NavManager.NavigateTo("/registration");
   }

   public async Task LoginHandler()
   {
      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
      {
         errorMessage = "Email or password is required";
         StateHasChanged();
         return;
      }

      if (selectedCurrency == default)
      {
         errorMessage = "please select currency";
      }

      AuthService authService = new AuthService();

      LoginDto loginDto = new LoginDto()
      {
         email = email,
         password = password
      };

      var loginSucess = await authService.Login(loginDto);
      
      if (loginSucess == null)
      {
         errorMessage = "Invalid username or password";
         StateHasChanged();
         return;
      }

      errorMessage = "";
      StateHasChanged();
      NavManager.NavigateTo("/counter");

   }

}