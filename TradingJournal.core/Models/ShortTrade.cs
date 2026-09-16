using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournal.core.Models
{
    public class ShortTrade : Trade
    {
        public override decimal CalculatePnL()
        {
            return Amount * (Entry - ExitPrice);
        }
    
    }
}
