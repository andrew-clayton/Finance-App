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
            AllotedAmount = 0;
            Type = Category.None;
        }
        public Budget(Category category)
        {
            AllotedAmount = 0;
            Type = Category.None;
            Type = category;
        }
        public int Id { get; set; }
        public Category Type{ get; set; }
        public float AllotedAmount { get; set; }

    }
}
