using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankSys.Models
{
    public class Detials
    {
        public int Id { get; set; }

        public string userId { get; set; }

        public DateTime date { get; set; }
        public string action { get; set; }
        public int amount { get; set; }
    }
}