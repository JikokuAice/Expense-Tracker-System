using ExpenseManagementSystem.DTOs.Dashboard;
using ExpenseManagementSystem.DTOs.Dashboard.Filters;
using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.Filters.Debts;
using ExpenseManagementSystem.Models;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Services.Interfaces;

namespace ExpenseManagementSystem.Services;

public class DashboardService(IDebtServices debtServices,IUserServices userServices,IGenericRepository genericRepository):IDashboardService
{
   private IDashboardService _dashboardServiceImplementation;


   public  async Task<DashboardDtoData> FetchDashboardData()
   {
      var userIdentifier = await userServices.FetchUserDetail();
      if (userIdentifier==null)
      {
         throw new Exception("User is not logged in");
      }

      var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);
      debtList = debtList.Where(x => x.CreatedBy == userIdentifier.Id).ToList();
      
      
      var transactionList = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);
      transactionList= transactionList.Where(x => x.CreatedBy == userIdentifier.Id).ToList();
      
      Console.WriteLine(debtList);
      Console.WriteLine(transactionList);
      return new DashboardDtoData()
      {
         TotalDebt = debtList.Sum(e => e.Amount),
         TotalInflow = transactionList.Where(e => e.Type == TransactionType.Inflows).Sum(e => e.Amount),
         TotalDebtCount = debtList.Count(),
         TotalOutflow = transactionList.Where(e=>e.Type==TransactionType.Outflows).Sum(e=>e.Amount),
         TotalInflowCount = transactionList.Count(e=>e.Type==TransactionType.Inflows),
         TotalOutflowCount = transactionList.Count(e=>e.Type==TransactionType.Outflows),
         
      };
   }

   public async Task<List<DetailsForVisualization>> GetInflowsTransactions(FilterForVisualization transactionFilterRequest)
   {
      var userIdentifier = await userServices.FetchUserDetail();

      if (userIdentifier == null)
      {
         throw new Exception("You are not logged in.");
      }
        
      var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);
      transactions= transactions.Where(x => x.CreatedBy == userIdentifier.Id).ToList();

      transactions = transactions.Where(x => x.Type == TransactionType.Inflows).ToList();
      

      var result = transactionFilterRequest.IsAscending 
         ? transactions.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
         : transactions.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

      return result.Select(x => new DetailsForVisualization()
      {
         Title = x.Title,
         Amount = x.Amount
      }).ToList();
   }

   public  async Task<List<DetailsForVisualization>> GetOutflowsTransactions(FilterForVisualization transactionFilterRequest)
   {
      var userIdentifier = await userServices.FetchUserDetail();

      if (userIdentifier == null)
      {
         throw new Exception("You are not logged in.");
      }
        
      var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);
      transactions= transactions.Where(x => x.CreatedBy == userIdentifier.Id).ToList();

      transactions = transactions.Where(x => x.Type == TransactionType.Outflows).ToList();
      

      var result = transactionFilterRequest.IsAscending 
         ? transactions.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
         : transactions.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

      return result.Select(x => new DetailsForVisualization()
      {
         Title = x.Title,
         Amount = x.Amount
      }).ToList();
     
   }

   public async Task<List<DetailsForVisualization>> GetDebtTransactions(FilterForVisualization transactionFilterRequest)
   {
      var userIdentifier = await userServices.FetchUserDetail();

      if (userIdentifier == null)
      {
         throw new Exception("You are not logged in.");
      }
        
      var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);
      debts= debts.Where(x => x.CreatedBy == userIdentifier.Id).ToList();


      var result = transactionFilterRequest.IsAscending 
         ? debts.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
         : debts.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

      return result.Select(x => new DetailsForVisualization()
      {
         Title = x.Title,
         Amount = x.Amount
      }).ToList();
      
   }

   public async Task<List<GetDebtDto>> GetDebtAsync(GetDebtFilterRequestDto debtFilterRequestDto)
   {
      
      var debtPendingFilterRequest = new GetDebtFilterRequestDto()
      {
         Status = DebtStatus.Pending,
         StartDate = debtFilterRequestDto.StartDate,
         EndDate = debtFilterRequestDto.EndDate,
         Search = debtFilterRequestDto.Search,
         IsDescending = debtFilterRequestDto.IsDescending,
         OrderBy = debtFilterRequestDto.OrderBy
      };


      var pendingDebt = await debtServices.GetAllDebts(debtPendingFilterRequest);

      var overDueFilterRequest = new GetDebtFilterRequestDto()
      {
         Status = DebtStatus.PastDue,
         Search = debtFilterRequestDto.Search,
         OrderBy = debtFilterRequestDto.OrderBy,
         EndDate = debtFilterRequestDto.EndDate,
         StartDate = debtFilterRequestDto.StartDate,
         IsDescending = debtFilterRequestDto.IsDescending
      };

      var overDueDebt = await debtServices.GetAllDebts(overDueFilterRequest);
      
      return pendingDebt.Concat(overDueDebt).ToList();
   }

   public async Task<string> FetchUserCurrencyName()
   {
      var userDetail = await userServices.FetchUserDetail();
      string userCurrencyName = userDetail.Currency;
      return userCurrencyName;
   }
}