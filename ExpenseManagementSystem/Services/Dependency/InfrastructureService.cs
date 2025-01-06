using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using ExpenseManagementSystem.Managers;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Services.Interfaces;
using ExpenseManagementSystemr.Services.Seed;

namespace ExpenseManagementSystem.Services.Dependency;

public static class InfrastructureService
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        #region Inject of Internal Services
        services.AddLocalization();

        services.AddMauiBlazorWebView();

        services.AddBlazoredLocalStorage();
        
        services.AddBlazorWebViewDeveloperTools();
        #endregion

        #region Mudblazor Service
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });
        #endregion

        #region Dependency Injection
        services.AddTransient<IlocalStorage, localStorage>();
        services.AddTransient<IserializerAndDeserializerManager, serializerAndDeserializerManager>();

        services.AddTransient<IGenericRepository, GenericRepository>();

        services.AddTransient<IAuthServices, AuthenticationService>();
        services.AddTransient<IDebtServices, DebtService>();
        services.AddTransient<ISeedingService, SeedingService>();
        services.AddTransient<ISnackBarServices, SnackbarService>();
        services.AddTransient<ITagServices, TagService>();
        services.AddTransient<ITransactionServices, TransactionService>();
        services.AddTransient<IUserServices, UserService>();
        #endregion
    }
}