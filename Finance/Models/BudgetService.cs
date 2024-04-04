using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Models
{
    public class BudgetService
    {
        public BudgetService()
        {
            if (!AreBudgetsInitializedForMonth(DateTime.Now)) InitializeBudgetsForMonth(DateTime.Now);
        }

        // Create new budgets for a given month
        public async Task InitializeBudgetsForMonth(DateTime timestamp)
        {
            if (AreBudgetsInitializedForMonth(timestamp)) return;
            using (var context = new FinanceContext())
            {
                // We need the list of all budgets
                var categories = Enum.GetValues(typeof(Category));

                // Create a budget for each one in the enumeration
                foreach (var category in categories)
                {
                    if (category is Category cat)
                    {
                        Budget currentBudget = new Budget(cat);
                        currentBudget.TimeStamp = timestamp;
                        context.Budgets.Add(currentBudget);
                        await context.SaveChangesAsync();
                    }
                    else throw new Exception("Unexpected");
                }

            }
        }

        // Add new singular budget
        public async Task AddBudget(Budget budget)
        {
            using (var context = new FinanceContext())
            {
                context.Budgets.Add(budget);
            }
        }


        // Check if budgets were initialized 
        bool AreBudgetsInitializedForMonth(DateTime timestamp)
        {
            using (var context = new FinanceContext())
            {
                IEnumerable<Budget> list = context.Budgets.Where(b => b.TimeStamp.Month == timestamp.Month && b.TimeStamp.Year == timestamp.Year);
                if (list.Count() == 0) return false;
                else return true;
            }
        }

        // Read
        public async Task<List<Budget>> GetAllBudgetsAsync()
        {
            if (!AreBudgetsInitializedForMonth(DateTime.Now)) {
                await InitializeBudgetsForMonth(DateTime.Now);
            }
            using (var context = new FinanceContext())
            {
                return await context.Budgets.ToListAsync();
            }
        }

        public async Task<Budget> FindBudgetWithCategory(Category type)
        {
            var budgetList = await GetAllBudgetsAsync();
            foreach (var budget in budgetList)
            {
                if (budget.Type == type)
                {
                    return budget;
                }
            }
            return null; // not supposed to happen
        }


        // Update
        public async Task UpdateBudget(Budget updatedBudget)
        {
            using (var context = new FinanceContext())
            {
                // Find the existing budget by ID
                Budget existingBudget = await context.Budgets.FindAsync(updatedBudget);
                if (existingBudget != null)
                {
                    existingBudget.AllotedAmount = updatedBudget.AllotedAmount;
                    // Mark the buddget as modified
                    context.Entry(existingBudget).State = EntityState.Modified;

                    // Save the changes to the database
                    await context.SaveChangesAsync();
                } else
                {
                    throw new KeyNotFoundException("Budget not found to update");
                }
            }
        }




        // Delete.. shouldn't be needed, right?
    }   
}