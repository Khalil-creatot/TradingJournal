using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.core.Models;

namespace TradingJournal.core.Services
{
    public class PositionSizeCalculator
    {
        public PositionSizeResult Calculate (
            decimal capital,
            decimal riskPercent,
            decimal entry,
            decimal stopLoss,
            decimal takeprofit,
            int leverage = 1)
        {
            var slDistance = Math.Abs(entry - stopLoss);
            if (slDistance == 0) throw new ArgumentException("Stoploss kan inte vara samma som Entry.");

            if (capital <= 0) throw new ArgumentException("Kapital måste vara större än noll.");
            
            if (riskPercent <= 0 || riskPercent > 100 ) throw new ArgumentException("Riskprocent måste vara mellan 0 och 100.");

            if (leverage <= 0) throw new ArgumentException("Leverage måste vara större än noll.");

            var totalRisk = capital * (riskPercent / 100);
            var amount = totalRisk / slDistance;
            var nominalPositionSize = amount * entry;
            var finalPositionSize = nominalPositionSize / leverage;
            var rr = Math.Abs(takeprofit - entry) / slDistance;


            // Return the result as a PositionSizeResult object
            return new PositionSizeResult
            {
               TotalRisk = Math.Round(totalRisk, 2), // Round to 2 decimal places for currency
               Amount = Math.Round(amount, 8), // Round to 8 decimal places for precision
               NominalPositionSize = Math.Round(nominalPositionSize, 2),
               FinalPositionSize = Math.Round(finalPositionSize, 2),
               RR = Math.Round(rr, 2)
            };
        }
    }
}
