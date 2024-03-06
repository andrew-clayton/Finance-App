using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Models
{
    public class FinanceContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public string DbPath { get; set; }

        public FinanceContext()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            DbPath = Path.Combine(folder, "finance.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("$Data Source={DbPath}");
        }

    }
}
