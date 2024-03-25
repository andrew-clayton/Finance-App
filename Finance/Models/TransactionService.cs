using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Models
{
    public class TransactionService
    {
        public TransactionService()
        {
        }

        // Read
        public async Task<List<ATransaction>> GetAllTransactionsAsync()
        {
            using (var context = new FinanceContext())
                return await context.Transactions.ToListAsync();
        }

        // Add
        public async Task AddTransaction(ATransaction transaction)
        {
            using (var context = new FinanceContext())
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
            return;
        }

        public async Task<List<ATransaction>> FindTransactionWithBudget(Category budget)
        {
            using (var context = new FinanceContext())
            {
                var budgetQuery =
                    from item in context.Transactions
                    where (item.Budget == budget)
                    select item;

                return await budgetQuery.ToListAsync();
            }
        }

        // Delete
        public async Task DeleteTransaction(int transactionId)
        {
            using (var context = new FinanceContext())
            {
                var transaction = await context.Transactions.FindAsync(transactionId);
                if (transaction != null)
                {

                    // remove the transaction from the context
                    context.Transactions.Remove(transaction);

                    // Asynchronously save the changes to the database
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateTransaction(ATransaction updatedTransaction)
        {
            using (var context = new FinanceContext())
            {
                // Find the existing transaction by id
                ATransaction existingTransaction = await context.Transactions.FindAsync(updatedTransaction);
                if (existingTransaction != null)
                {
                    existingTransaction.Value = updatedTransaction.Value;
                    existingTransaction.TimeStamp = updatedTransaction.TimeStamp;
                    existingTransaction.Budget = updatedTransaction.Budget;
                    existingTransaction.IsReoccuring = updatedTransaction.IsReoccuring;
                    existingTransaction.Title = updatedTransaction.Title;
                    existingTransaction.Description = updatedTransaction.Description;

                    // Mark the transaction as modified
                    context.Entry(existingTransaction).State = EntityState.Modified;

                    // Save the changes to the database
                    await context.SaveChangesAsync();
                }
                else
                {
                    // handle the case where the transaction doesn't exist
                    throw new KeyNotFoundException("Transaction not found for update");
                }
            }
        }

        public int GetTransactionCount()
        {
            using var context = new FinanceContext();
            return context.Transactions.Count();
        }

        public async Task<List<ATransaction>> GetRevenueListAsync()
        {
            var transactionList = await GetAllTransactionsAsync();
            var revenueList = new List<ATransaction>();

            foreach (var item in transactionList)
            {
                if (item.Value > 0) revenueList.Add(item);
            }
            return revenueList;
        }
        public async Task<List<ATransaction>> GetExpenseListAsync()
        {
            var transactionList = await GetAllTransactionsAsync();
            var expenseList = new List<ATransaction>();

            foreach (var item in transactionList)
            {
                if (item.Value < 0)
                {
                    expenseList.Add(item);
                }
            }
            return expenseList;
        }


        // 
    }
}
