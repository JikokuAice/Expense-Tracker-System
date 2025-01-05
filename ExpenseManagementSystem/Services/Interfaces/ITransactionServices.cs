using ExpenseManagementSystem.DTOs.Transaction;
using ExpenseManagementSystem.Filters.Transactions;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface ITransactionServices
{
    Task<decimal> GetUserReminingBalance();
    
    Task<TransactionCountGetDto> TransactionCountGet();

    GetTransactionDto GetTransactionById(Guid transactionId);
    
    Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task InsertIntoTransaction(InsertIntoTransactionDto transaction);

    Task UpdateTransaction(UpdateTransactionDto transaction);

    void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
