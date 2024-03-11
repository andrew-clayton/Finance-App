using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Finance.Models
{
    public class FinanceInfoTest
    {
        private TransactionService transactionService = new TransactionService(new FinanceContext());
        List<ATransaction> transactionList = new List<ATransaction>();

        FinanceInfoTest()
        {
        }
        private async void RefreshListOfAll()
        {
            transactionList = await transactionService.GetAllTransactionsAsync();
        }

        public float FindListTotal(List<ATransaction> list)
        {
            float total = 0;
            foreach (var item in list)
            {
                total += item.Value;
            }
            return total;
        }
        public List<ATransaction> SimulateTransactionsDone()
        {
            string[] reasons = { "Insurance", "Groceries", "Dining Out", "Cane's", "Target", "Costco", "Tuition" };
            Category[] categories = { Category.None, Category.Grocery, Category.DiningOut, Category.Misc, Category.Personal, Category.Utility };
            List<ATransaction> transactionList = new List<ATransaction>();
            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                transactionList.Add(new ATransaction()
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

            return transactionList;
        }
    }
}
