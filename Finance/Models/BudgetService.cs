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
        }

        // Create new budgets
        public async Task InitializeBudgets()
        {
            if (AreBudgetsInitialized()) return;
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
                        context.Budgets.Add(currentBudget);
                        await context.SaveChangesAsync();
                    }
                    else throw new Exception("Unexpected");
                }

            }
        }


        // Check if budgets were initialized 
        bool AreBudgetsInitialized()
        {
            using (var context = new FinanceContext())
            {
                return context.Budgets.Any();
            }
        }

        // Read
        public async Task<List<Budget>> GetAllBudgetsAsync()
        {
            if (!AreBudgetsInitialized()) {
                InitializeBudgets();
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