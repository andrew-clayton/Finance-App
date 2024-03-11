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
        private readonly FinanceContext _context;
        public TransactionService(FinanceContext context)
        {
            _context = context;
        }

        // Read
        public async Task<List<ATransaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        // Add
        public async Task AddTransaction(ATransaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ATransaction>> FindTransactionWithBudget(Category budget)
        {
            var budgetQuery = 
                from item in _context.Transactions 
                where (item.Budget == budget)
                select item;

            return await budgetQuery.ToListAsync();
        }

        // Delete
        public async Task DeleteTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction != null)
            {
                // remove the transaction from the context
                _context.Transactions.Remove(transaction);

                // Asynchronously save the changes to the database
                await _context.SaveChangesAsync();
            }
        }

        public int GetTransactionCount()
        {
            return _context.Transactions.Count();
        }


        // 
    }
}
