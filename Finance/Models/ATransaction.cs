using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Models
{
    public class ATransaction
    {
        public int Id { get; set; }
        public float Value { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsReoccuring { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Category Budget { get; set; }
    }
}