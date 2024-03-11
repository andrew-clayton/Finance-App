using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ControlzEx.Theming;
using Finance;
using Finance.Models;

namespace Finance.Test
{
    public class FinanceInfoTest
    {
        private TransactionService transactionService = new TransactionService(new FinanceContext());
        List<ATransaction> transactionList = new List<ATransaction>();

        public FinanceInfoTest()
        {

        }
        private async void RefreshList()
        {
            transactionList = await transactionService.GetAllTransactionsAsync();
        }

        public float FindListTotal()
        {
            float total = 0;
            foreach (var item in transactionList)
            {
                total += item.Value;
            }
            return total;
        }
        public async void SimulateTransactionsDone()
        {
            if (transactionService.GetTransactionCount() != 0) return;

            string[] reasons = { "Insurance", "Trader Joe's", "Cane's", "Target", "Costco", "Tuition" };
            Category[] categories = { Category.None, Category.Grocery, Category.DiningOut, Category.Misc, Category.Personal, Category.Utility };
            var random = new Random();

            for (int i = 0; i < 50; i++)
            {
                await transactionService.AddTransaction(new ATransaction()
                {
                    Id = i,
                    Value = random.Next(-500, 500),
                    TimeStamp = DateTime.Now.AddDays(random.Next(-30, 0)),
                    IsReoccuring = false,
                    Title = reasons[random.Next(0, reasons.Length)],
                    Description = String.Empty,
                    Budget = categories[random.Next(0, categories.Length)]
                });
            }

            RefreshList();
        }

        public async void ShowGeneralInformation()
        {
            var groceryTransactions = await transactionService.FindTransactionWithBudget(Category.Grocery);

            Console.WriteLine($"The total net value of all {transactionList.Count} transactions is {FindListTotal()}");
            Console.WriteLine($"There were {groceryTransactions.Count} transactions in the grocery budget.");
        }
    }
}
