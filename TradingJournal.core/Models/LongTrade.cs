using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournal.core.Models
{
    public class LongTrade : Trade
    {
        public override decimal CalculatePnL()
        {
            return Amount * (ExitPrice - Entry);
        }
    }
}
