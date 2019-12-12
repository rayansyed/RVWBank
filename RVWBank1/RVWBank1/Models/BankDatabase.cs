using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVWBank1.Models
{
    public class BankDatabase : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public BankDatabase(DbContextOptions<BankDatabase> options) : base(options)
        {

        }
    



    }
}
