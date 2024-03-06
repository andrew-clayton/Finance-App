using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Models
{
    public class Budget
    {
        public Category BudgetCategory{ get; set; }
        public float BudgetAmount { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        [NotMapped]
        public float SpentAmount => Transactions.Sum(t => t.Value);
    }
}
