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
        //private readonly FinanceContext _context;
        public TransactionService()
        {
            //_context = context;
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
