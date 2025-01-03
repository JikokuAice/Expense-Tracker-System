using ExpenseTrackerSystem.Models.dto;
using ExpenseTrackerSystem.Models.Enums;
using ExpenseTrackerSystem.Services;
using Microsoft.AspNetCore.Components;

namespace ExpenseTrackerSystem.Components.Pages;

public partial class Registration : ComponentBase
{

   private string? name = "";
   private string? email  = "";
   private string? password  = "";
   private string? confirmPassword  = "";
   private string? errorMessage = "";
   
   private void NavigateTologinPage()
   {
      NavManager.NavigateTo("/login");
   }
   
   private async Task RegisterHandler()
   {
      if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)||string.IsNullOrEmpty(confirmPassword))
      { 
         errorMessage = "fill all fields!";
         StateHasChanged(); 
         return;
      }
      
      if (confirmPassword != password)
      {
         errorMessage = "confirm Password do not match!";
         StateHasChanged(); 
         return;
      }
      
      AuthService authService = new AuthService();

      RegistrationDto registrationDto = new RegistrationDto()
      {
      Email = email,
      Password = password,
      Name = name,
      CreationDate = DateTime.Now   
      };
      
   var registered = await   authService.Register(registrationDto);
   
   if (registered)
   {
      errorMessage = "";
      StateHasChanged();
      NavManager.NavigateTo("/login");
   }
   
   errorMessage="User already exist";
   StateHasChanged();
   }
   
}