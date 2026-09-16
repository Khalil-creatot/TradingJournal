using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournal.core.Models
{
    // This class is used to return the result of the position size calculation
    public class PositionSizeResult
    {
        public decimal TotalRisk { get; set; }
        public decimal Amount { get; set; }
        
        public decimal NominalPositionSize { get; set; }
        public decimal FinalPositionSize { get; set; }
        public decimal RR { get; set; }
    }
}
