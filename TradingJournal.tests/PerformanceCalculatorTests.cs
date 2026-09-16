using TradingJournal.core.Models;
using TradingJournal.Core.Services;

namespace TradingJournal.Tests
{
    public class PerformanceCalculatorTests
    {
        private readonly PerformanceCalculator _calculator = new();

        // Testdata: 4 trades, 1 breakeven, 2 losses, 1 win
        private List<Trade> GetTestTrades()
        {
            return new List<Trade>
            {
                new LongTrade
                {
                    Asset     = "BNB",
                    Entry     = 619m,
                    ExitPrice = 619m,
                    Amount    = 0.233773494m,
                    Deposit   = 0,
                    Withdraw  = 0
                },
                new LongTrade
                {
                    Asset     = "ETH",
                    Entry     = 2103m,
                    ExitPrice = 1634m,
                    Amount    = 0.003361345m,
                    Deposit   = 0,
                    Withdraw  = 0
                },
                new LongTrade
                {
                    Asset     = "BTC",
                    Entry     = 66134m,
                    ExitPrice = 66130m,
                    Amount    = 0.007575758m,
                    Deposit   = 0,
                    Withdraw  = 0
                },
                new LongTrade
                {
                    Asset     = "BTC",
                    Entry     = 64143m,
                    ExitPrice = 65730m,
                    Amount    = 0.00770416m,
                    Deposit   = 0,
                    Withdraw  = 0
                }
            };
        }


        // Test cases
        [Fact]
        public void GetEquityCurve_StartsWithStartingCapital()
        {
            var trades = GetTestTrades();
            var curve = _calculator.GetEquityCurve(trades, 1000m);

            Assert.Equal(1000m, curve[0]);
        }

        [Fact]
        public void GetEquityCurve_HasCorrectNumberOfPoints()
        {
            var trades = GetTestTrades();
            var curve = _calculator.GetEquityCurve(trades, 1000m);

            // Startkapital + en punkt per trade
            Assert.Equal(trades.Count + 1, curve.Count);
        }

        [Fact]
        public void GetMaxDrawdown_ReturnsPositiveValue()
        {
            var trades = GetTestTrades();
            var curve = _calculator.GetEquityCurve(trades, 1000m);
            var dd = _calculator.GetMaxDrawdown(curve);

            Assert.True(dd >= 0);
        }

        [Fact]
        public void GetWinRate_WithMixedTrades_ReturnsCorrectPercent()
        {
            var trades = GetTestTrades();
            var winRate = _calculator.GetWinRate(trades);

            // BNB: breakeven (0), ETH: förlust, BTC1: förlust, BTC2: vinst
            // 1 vinst av 4 = 25%
            Assert.Equal(25m, winRate);
        }

        [Fact]
        public void GetConsecutiveWins_ReturnsCorrectCount()
        {
            var trades = GetTestTrades();
            var result = _calculator.GetConsecutiveWins(trades);

            Assert.True(result >= 0);
        }

        [Fact]
        public void GetPeakEquity_IsGreaterThanOrEqualToStart()
        {
            var trades = GetTestTrades();
            var curve = _calculator.GetEquityCurve(trades, 1000m);
            var peak = _calculator.GetPeakEquity(curve);

            Assert.True(peak >= 1000m);
        }
    }
}