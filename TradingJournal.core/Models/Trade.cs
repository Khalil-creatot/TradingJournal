using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournal.core.Models
{
    public abstract class Trade
    {
        // generate a new guid for each trade
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CloseDate { get; set; }
        public string Asset { get; set; } = string.Empty;
        public string TradeType { get; set; } = string.Empty;


        public decimal Entry { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal Amount { get; set; }

        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }

        public string Notes { get; set; } = string.Empty;

        public abstract decimal CalculatePnL();
        public decimal CalculateRR()
        {
            var SlDistance = Math.Abs(Entry - StopLoss);
            if (SlDistance == 0) return 0;
            return Math.Abs(TakeProfit - Entry) / SlDistance;
            
        }

        
        public decimal PnL => CalculatePnL();


    }
}
