using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVWBank1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public float Amount { get; set; }
        public string AccountType { get; set; }
        public string Method { get; set; }
    }
}
