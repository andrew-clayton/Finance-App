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
        public int BudgetId { get; set; }
        public Category BudgetCategory{ get; set; }
        public float BudgetAmount { get; set; }
        public List<ATransaction> Transactions { get; set; } = new List<ATransaction>();

        [NotMapped]
        public float SpentAmount => Transactions.Sum(t => t.Value);
    }
}
