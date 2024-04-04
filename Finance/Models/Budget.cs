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
        public Budget()
        {
            AllottedAmount = 0;
            Type = Category.None;
            TimeStamp = DateTime.Now;
        }
        public Budget(Category category)
        {
            AllottedAmount = 0;
            Type = Category.None;
            Type = category;
            TimeStamp = DateTime.Now;
        }
        public int Id { get; set; }
        public Category Type{ get; set; }
        public float AllottedAmount { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
