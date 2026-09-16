using TradingJournal.core.Models;


namespace TradingJournal.Core.Services
{
    public class PerformanceCalculator
    {
        // Equity curve exklusive insättningar/uttag
        public List<decimal> GetEquityCurve(
            IEnumerable<Trade> trades,
            decimal startingCapital)
        {
            var curve = new List<decimal> { startingCapital };
            var balance = startingCapital;

            foreach (var trade in trades)
            {
                balance += trade.CalculatePnL();
                curve.Add(balance);
            }

            return curve;
        }

        // Equity curve inklusive insättningar/uttag (Overall)
        public List<decimal> GetEquityCurveOverall(
            IEnumerable<Trade> trades,
            decimal startingCapital)
        {
            var curve = new List<decimal> { startingCapital };
            var balance = startingCapital;

            foreach (var trade in trades)
            {
                balance += trade.CalculatePnL()
                         + trade.Deposit
                         - trade.Withdraw;
                curve.Add(balance);
            }

            return curve;
        }

        // Max drawdown i procent
        public decimal GetMaxDrawdown(List<decimal> curve)
        {
            if (curve == null || curve.Count == 0)
                return 0;

            var peak = curve[0];
            var maxDrawdown = 0m;

            foreach (var balance in curve)
            {
                if (balance > peak)
                    peak = balance;

                var drawdown = peak > 0
                    ? (peak - balance) / peak
                    : 0;

                if (drawdown > maxDrawdown)
                    maxDrawdown = drawdown;
            }

            return Math.Round(maxDrawdown * 100, 4);
        }

        // Expectancy = (WinRate x AvgWin) - (LossRate x AvgLoss)
        public decimal GetExpectancy(IEnumerable<Trade> trades)
        {
            var tradeList = trades.ToList();
            if (!tradeList.Any()) return 0;

            var wins = tradeList.Where(t => t.CalculatePnL() > 0).ToList();
            var losses = tradeList.Where(t => t.CalculatePnL() < 0).ToList();

            if (!wins.Any() || !losses.Any()) return 0;

            var winRate = (decimal)wins.Count / tradeList.Count;
            var lossRate = 1 - winRate;
            var avgWin = wins.Average(t => t.CalculatePnL());
            var avgLoss = Math.Abs(losses.Average(t => t.CalculatePnL()));

            return Math.Round((winRate * avgWin) - (lossRate * avgLoss), 2);
        }

        // Win/Loss Ratio = AvgWin / AvgLoss
        public decimal GetWinLossRatio(IEnumerable<Trade> trades)
        {
            var tradeList = trades.ToList();

            var wins = tradeList.Where(t => t.CalculatePnL() > 0).ToList();
            var losses = tradeList.Where(t => t.CalculatePnL() < 0).ToList();

            if (!wins.Any() || !losses.Any()) return 0;

            var avgWin = wins.Average(t => t.CalculatePnL());
            var avgLoss = Math.Abs(losses.Average(t => t.CalculatePnL()));

            return Math.Round(avgWin / avgLoss, 2);
        }

        // Peak equity value
        public decimal GetPeakEquity(List<decimal> curve)
        {
            return curve.Any() ? curve.Max() : 0;
        }

        // Lowest equity value
        public decimal GetLowestEquity(List<decimal> curve)
        {
            return curve.Any() ? curve.Min() : 0;
        }

        // Consecutive wins
        public int GetConsecutiveWins(IEnumerable<Trade> trades)
        {
            var max = 0;
            var current = 0;

            foreach (var trade in trades)
            {
                if (trade.CalculatePnL() > 0)
                {
                    current++;
                    if (current > max) max = current;
                }
                else
                {
                    current = 0;
                }
            }

            return max;
        }

        // Consecutive losses
        public int GetConsecutiveLosses(IEnumerable<Trade> trades)
        {
            var max = 0;
            var current = 0;

            foreach (var trade in trades)
            {
                if (trade.CalculatePnL() < 0)
                {
                    current++;
                    if (current > max) max = current;
                }
                else
                {
                    current = 0;
                }
            }

            return max;
        }

        // Win rate i procent
        public decimal GetWinRate(IEnumerable<Trade> trades)
        {
            var tradeList = trades.ToList();
            if (!tradeList.Any()) return 0;

            var wins = tradeList.Count(t => t.CalculatePnL() > 0);
            return Math.Round((decimal)wins / tradeList.Count * 100, 2);
        }
    }
}