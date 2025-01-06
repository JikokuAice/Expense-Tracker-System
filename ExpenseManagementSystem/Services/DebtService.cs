using ExpenseManagementSystem.Models;
using ExpenseManagementSystem.DTOs.Debts;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Filters.Debts;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.Services.Interfaces;
using ExpenseManagementSystemr.DTOs.Debts;

namespace ExpenseManagementSystem.Services;

public class DebtService(IGenericRepository genericRepository, IUserServices userService) : IDebtServices
{
    public async Task<decimal> GetUserReminingBalance()
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();
        
        var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debtList = debtList.Where(x => x.CreatedBy == userDetails.Id).ToList();
        
        return transactions.Where(x => x.Type == TransactionType.Inflows).Sum(x => x.Amount) -
               transactions.Where(x => x.Type == TransactionType.Outflows).Sum(x => x.Amount) -
               debtList.Where(x => x.Status == DebtStatus.Cleared).Sum(x => x.Amount);
    }
    
    public async Task<decimal> FtechPendingDebtTotalAmt()
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debtList = debtList.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return debtList.Where(x => x.Status == DebtStatus.Pending).Sum(x => x.Amount);
    }

    public async Task<GetDebtsCountDto> GetDebtsCount()
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debtList = debtList.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return new GetDebtsCountDto
        {
            All = debtList.Count,
            Cleared = debtList.Count(x => x.Status == DebtStatus.Cleared),
            Pending = debtList.Count(x => x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now)),
            PastDue = debtList.Count(x => x.Status != DebtStatus.Cleared && x.DueDate <= DateOnly.FromDateTime(DateTime.Now))
        };
    }
    
    public GetDebtDto FetchDebtById(Guid id)
    {
        var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debt = debtList.FirstOrDefault(x => x.Id == id);

        if (debt == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        return new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
            ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Status = debt.Status != DebtStatus.Cleared 
                ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now) 
                    ? DebtStatus.PastDue 
                    : DebtStatus.Pending 
                : DebtStatus.Cleared
        };
    }

    public async Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest)
    {
        var debtList = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);
        
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        debtList = debtList.Where(x => x.CreatedBy == userDetails.Id).ToList();

        if (!string.IsNullOrEmpty(debtFilterRequest.Search))
        {
            debtList = debtList.Where(x => x.Title.Contains(debtFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (debtFilterRequest.Status != null)
        {
            if (debtFilterRequest.Status == DebtStatus.Pending)
            {
                debtList = debtList.Where(x => x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else if (debtFilterRequest.Status == DebtStatus.PastDue)
            {
                debtList = debtList.Where(x => x.Status != DebtStatus.Cleared && x.DueDate < DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else if (debtFilterRequest.Status == DebtStatus.Cleared)
            {
                debtList = debtList.Where(x => x.Status == DebtStatus.Cleared).ToList();
            }
        }
        
        if (debtFilterRequest.StartDate != null)
        {
            debtList = debtList.Where(x => x.DueDate >= DateOnly.FromDateTime(debtFilterRequest.StartDate.Value)).ToList();
        }

        if (debtFilterRequest.EndDate != null)
        {
            debtList = debtList.Where(x => x.DueDate < DateOnly.FromDateTime(debtFilterRequest.EndDate.Value)).ToList();
        }
        
        if (!string.IsNullOrEmpty(debtFilterRequest.OrderBy))
        {
            debtList = debtFilterRequest.OrderBy switch
            {
                "Title" => debtFilterRequest.IsDescending ? debtList.OrderByDescending(x => x.Title).ToList() : debtList.OrderBy(x => x.Title).ToList(),
                "Amount" => debtFilterRequest.IsDescending ? debtList.OrderByDescending(x => x.Amount).ToList() : debtList.OrderBy(x => x.Amount).ToList(),
                "DueDate" => debtFilterRequest.IsDescending ? debtList.OrderByDescending(x => x.DueDate).ToList() : debtList.OrderBy(x => x.DueDate).ToList(),
                "ClearedDate" => debtFilterRequest.IsDescending ? debtList.OrderByDescending(x => x.ClearedDate).ToList() : debtList.OrderBy(x => x.ClearedDate).ToList(),
                _ => debtList
            };
        }
        
        return debtList.Select(debt => new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
            ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Status = debt.Status != DebtStatus.Cleared 
                ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now) 
                    ? DebtStatus.PastDue 
                    : DebtStatus.Pending 
                : DebtStatus.Cleared
        }).ToList();
    }

    public async Task InsertUserDebt(InsertUserDebtDto debt)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = new Debt
        {
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate != null ? DateOnly.FromDateTime(debt.DueDate.Value) : DateOnly.FromDateTime(DateTime.Now),
            Status = DebtStatus.Pending,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts.Add(debtModel);

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task UpdateUserDebt(UpdateUserDebtDto debt)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.Title = debt.Title;
        debtModel.Source = debt.Source;
        debtModel.Amount = debt.Amount;
        debtModel.DueDate = debt.DueDate != null
            ? DateOnly.FromDateTime(debt.DueDate.Value)
            : DateOnly.FromDateTime(DateTime.Now);
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task ClearUserDebt(Guid debtId)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debtId);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        if (debtModel.Status == DebtStatus.Cleared)
        {
            throw new Exception("You can not clear an already cleared debt.");
        }

        var balance = await GetUserReminingBalance();

        if (balance < debtModel.Amount)
        {
            throw new Exception("You do not have sufficient balance to clear the following debt, please add sources of incoming transactions to clear the respective debt.");
        } 
        
        debtModel.Status = DebtStatus.Cleared;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;
        debtModel.ClearedDate = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.IsActive = false;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }
}
