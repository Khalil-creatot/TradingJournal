using System.Windows.Input;
using TradingJournal.core.Services;
using TradingJournal.Core.Services;
using TradingJournal.wpf.Helpers;

namespace TradingJournal.wpf.ViewModels
{
    public class PositionCalculatorViewModel : BaseViewModel
    {
        private readonly PositionSizeCalculator _calculator = new();

        private decimal _capital;
        public decimal Capital
        {
            get => _capital;
            set => SetProperty(ref _capital, value);
        }

        private decimal _riskPercent = 1;
        public decimal RiskPercent
        {
            get => _riskPercent;
            set => SetProperty(ref _riskPercent, value);
        }

        private decimal _entry;
        public decimal Entry
        {
            get => _entry;
            set => SetProperty(ref _entry, value);
        }

        private decimal _stopLoss;
        public decimal StopLoss
        {
            get => _stopLoss;
            set => SetProperty(ref _stopLoss, value);
        }

        private decimal _takeProfit;
        public decimal TakeProfit
        {
            get => _takeProfit;
            set => SetProperty(ref _takeProfit, value);
        }

        private int _leverage = 1;
        public int Leverage
        {
            get => _leverage;
            set => SetProperty(ref _leverage, value);
        }

        private decimal _totalRisk;
        public decimal TotalRisk
        {
            get => _totalRisk;
            set => SetProperty(ref _totalRisk, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private decimal _nominalPositionSize;
        public decimal NominalPositionSize
        {
            get => _nominalPositionSize;
            set => SetProperty(ref _nominalPositionSize, value);
        }

        private decimal _finalPositionSize;
        public decimal FinalPositionSize
        {
            get => _finalPositionSize;
            set => SetProperty(ref _finalPositionSize, value);
        }

        private decimal _rr;
        public decimal RR
        {
            get => _rr;
            set => SetProperty(ref _rr, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand CalculateCommand { get; }
        public ICommand ResetCommand { get; }

        public PositionCalculatorViewModel()
        {
            CalculateCommand = new RelayCommand(_ => Calculate());
            ResetCommand = new RelayCommand(_ => Reset());
        }

        private void Calculate()
        {
            ErrorMessage = string.Empty;

            if (Capital <= 0 || Entry <= 0 ||
                StopLoss <= 0 || TakeProfit <= 0 ||
                StopLoss == Entry)
            {
                ErrorMessage = "Fyll i alla fält korrekt innan du räknar.";
                return;
            }

            try
            {
                var result = _calculator.Calculate(
                    Capital, RiskPercent, Entry,
                    StopLoss, TakeProfit, Leverage);

                TotalRisk = result.TotalRisk;
                Amount = result.Amount;
                NominalPositionSize = result.NominalPositionSize;
                FinalPositionSize = result.FinalPositionSize;
                RR = result.RR;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void Reset()
        {
            Capital = 0;
            RiskPercent = 1;
            Entry = 0;
            StopLoss = 0;
            TakeProfit = 0;
            Leverage = 1;
            TotalRisk = 0;
            Amount = 0;
            NominalPositionSize = 0;
            FinalPositionSize = 0;
            RR = 0;
            ErrorMessage = string.Empty;
        }
    }
}