using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RVWBank1.Models
{
    public class Account
    {
        public int Id { get; set; }
        public float Checkings { get; set; }
        public float Savings { get; set; }
        public float Invested { get; set; }
        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

 

    }
}
