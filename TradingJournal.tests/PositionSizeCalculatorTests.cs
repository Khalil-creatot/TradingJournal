using TradingJournal.core.Services;
using TradingJournal.core.Models;
using TradingJournal.core.Services;

namespace TradingJournal.Tests
{
    public class PositionSizeCalculatorTests
    {
        private readonly PositionSizeCalculator _calculator = new();

        // ── Testar BTC-traden från din Excel (rad 10) ──
        // Entry: 64143, SL: 62845, TP: 67009
        // Kapital: $1000, Risk: 1%, Leverage: 10
        // Förväntat: FinalPS ≈ $49
        [Fact]
        public void Calculate_BtcTrade_ReturnCorrectFinalPositionSize()
        {
            var result = _calculator.Calculate(
                capital: 1000m,
                riskPercent: 1m,
                entry: 64143m,
                stopLoss: 62845m,
                takeProfit: 67009m,
                leverage: 10);

            Assert.Equal(49m, result.FinalPositionSize, precision: 0);
        }

        // ── Testar ETH-traden från din Excel (rad 11) ──
        // Entry: 2103, SL: 1634, TP: 2010
        // Kapital: $1092, Risk: 1%, Leverage: 1
        [Fact]
        public void Calculate_EthTrade_ReturnCorrectAmount()
        {
            var result = _calculator.Calculate(
                capital: 1092m,
                riskPercent: 1m,
                entry: 2103m,
                stopLoss: 1634m,
                takeProfit: 2010m,
                leverage: 1);

            Assert.True(result.Amount > 0);
            Assert.Equal(10.92m / 469m, result.Amount, precision: 4);
        }

        // ── Testar att TotalRisk beräknas korrekt ──
        [Fact]
        public void Calculate_TotalRisk_IsCorrect()
        {
            var result = _calculator.Calculate(
                capital: 1000m,
                riskPercent: 1m,
                entry: 64143m,
                stopLoss: 62845m,
                takeProfit: 67009m,
                leverage: 10);

            Assert.Equal(10m, result.TotalRisk);
        }

        // ── Testar R/R-kvoten ──
        // |TP - Entry| / |Entry - SL| = |67009 - 64143| / |64143 - 62845|
        // = 2866 / 1298 ≈ 2.21
        [Fact]
        public void Calculate_RR_IsCorrect()
        {
            var result = _calculator.Calculate(
                capital: 1000m,
                riskPercent: 1m,
                entry: 64143m,
                stopLoss: 62845m,
                takeProfit: 67009m,
                leverage: 10);

            Assert.Equal(2.21m, result.RR);
        }

        // ── Testar felhantering: SL = Entry ──
        [Fact]
        public void Calculate_WhenSlEqualsEntry_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _calculator.Calculate(
                    capital: 1000m,
                    riskPercent: 1m,
                    entry: 64143m,
                    stopLoss: 64143m,
                    takeProfit: 67009m));
        }

        // ── Testar felhantering: negativt kapital ──
        [Fact]
        public void Calculate_WhenCapitalIsZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _calculator.Calculate(
                    capital: 0m,
                    riskPercent: 1m,
                    entry: 64143m,
                    stopLoss: 62845m,
                    takeProfit: 67009m));
        }

        // ── Testar felhantering: ogiltig risk% ──
        [Fact]
        public void Calculate_WhenRiskPercentIsOver100_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _calculator.Calculate(
                    capital: 1000m,
                    riskPercent: 150m,
                    entry: 64143m,
                    stopLoss: 62845m,
                    takeProfit: 67009m));
        }
    }
}