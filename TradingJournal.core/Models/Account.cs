using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournal.core.Models
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal StartingCapital { get; set; }
        public decimal CurrentBalance { get; set; }
        
        public ICollection<Trade> Trades { get; set; } = new List<Trade>();
    }
}
