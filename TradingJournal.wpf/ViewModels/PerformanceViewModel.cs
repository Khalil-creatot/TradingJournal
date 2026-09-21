using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingJournal.core.Interfaces;
using TradingJournal.Core.Services;
using TradingJournal.wpf.Helpers;

namespace TradingJournal.wpf.ViewModels
{
    public class PerformanceViewModel : BaseViewModel
    {
        private readonly ITradeRepository _repository;
        private readonly PerformanceCalculator _calculator = new();

        public ObservableCollection<EquityPoint> EquityCurve { get; set; } = new();
        public ObservableCollection<EquityPoint> EquityCurveOverall { get; set; } = new();

        private decimal _expectancy;
        public decimal Expectancy
        {
            get => _expectancy;
            set => SetProperty(ref _expectancy, value);
        }

        private decimal _winRate;
        public decimal WinRate
        {
            get => _winRate;
            set => SetProperty(ref _winRate, value);
        }

        private decimal _winLossRatio;
        public decimal WinLossRatio
        {
            get => _winLossRatio;
            set => SetProperty(ref _winLossRatio, value);
        }

        private decimal _maxDrawdown;
        public decimal MaxDrawdown
        {
            get => _maxDrawdown;
            set => SetProperty(ref _maxDrawdown, value);
        }

        private decimal _peakEquity;
        public decimal PeakEquity
        {
            get => _peakEquity;
            set => SetProperty(ref _peakEquity, value);
        }

        private decimal _lowestEquity;
        public decimal LowestEquity
        {
            get => _lowestEquity;
            set => SetProperty(ref _lowestEquity, value);
        }

        private int _consecutiveWins;
        public int ConsecutiveWins
        {
            get => _consecutiveWins;
            set => SetProperty(ref _consecutiveWins, value);
        }

        private int _consecutiveLosses;
        public int ConsecutiveLosses
        {
            get => _consecutiveLosses;
            set => SetProperty(ref _consecutiveLosses, value);
        }

        private int _totalTrades;
        public int TotalTrades
        {
            get => _totalTrades;
            set => SetProperty(ref _totalTrades, value);
        }

        private decimal _totalPnL;
        public decimal TotalPnL
        {
            get => _totalPnL;
            set => SetProperty(ref _totalPnL, value);
        }

        private PlotModel _plotModel = new();
        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }

        public ICommand RefreshCommand { get; }

        public PerformanceViewModel()
        {
            _repository = (ITradeRepository)App.ServiceProvider
                .GetService(typeof(ITradeRepository))!;

            RefreshCommand = new RelayCommand(_ => LoadData());

            var count = _repository.GetAll().Count();
            System.Diagnostics.Debug.WriteLine($"Antal trades i databasen: {count}");

            LoadData();
        }

        private void LoadData()
        {
            var trades = _repository.GetAll()
                .OrderBy(t => t.CloseDate)
                .ToList();

            TotalTrades = trades.Count;

            if (!trades.Any()) return;

            var startCapital = 1000m;

            var curve = _calculator.GetEquityCurve(trades, startCapital);
            var curveOverall = _calculator.GetEquityCurveOverall(trades, startCapital);

            EquityCurve.Clear();
            for (int i = 0; i < curve.Count; i++)
                EquityCurve.Add(new EquityPoint { Index = i, Balance = curve[i] });

            EquityCurveOverall.Clear();
            for (int i = 0; i < curveOverall.Count; i++)
                EquityCurveOverall.Add(new EquityPoint { Index = i, Balance = curveOverall[i] });

            TotalPnL = trades.Sum(t => t.CalculatePnL());
            Expectancy = _calculator.GetExpectancy(trades);
            WinRate = _calculator.GetWinRate(trades);
            WinLossRatio = _calculator.GetWinLossRatio(trades);
            MaxDrawdown = _calculator.GetMaxDrawdown(curve);
            PeakEquity = _calculator.GetPeakEquity(curve);
            LowestEquity = _calculator.GetLowestEquity(curve);
            ConsecutiveWins = _calculator.GetConsecutiveWins(trades);
            ConsecutiveLosses = _calculator.GetConsecutiveLosses(trades);

            BuildPlot(curve, curveOverall);
        }

        private void BuildPlot(List<decimal> curve, List<decimal> curveOverall)
        {
            var model = new PlotModel
            {
                Background = OxyColor.FromArgb(0, 0, 0, 0),
                PlotAreaBorderColor = OxyColor.FromRgb(226, 232, 240),
                TextColor = OxyColor.FromRgb(148, 163, 184),
                IsLegendVisible = true
            };

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineColor = OxyColor.FromRgb(226, 232, 240),
                TicklineColor = OxyColor.FromRgb(226, 232, 240),
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(241, 245, 249),
                LabelFormatter = value => $"${value:N0}"
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                AxislineColor = OxyColor.FromRgb(226, 232, 240),
                TicklineColor = OxyColor.FromRgb(226, 232, 240),
                Title = "Trade #",
                TitleColor = OxyColor.FromRgb(148, 163, 184)
            });

            var series1 = new LineSeries
            {
                Title = "Balance (exkl. D/W)",
                Color = OxyColor.FromRgb(37, 99, 235),
                StrokeThickness = 2.5,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColor.FromRgb(37, 99, 235),
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1.5
            };
            for (int i = 0; i < curve.Count; i++)
                series1.Points.Add(new DataPoint(i, (double)curve[i]));

            var series2 = new LineSeries
            {
                Title = "Balance (overall)",
                Color = OxyColor.FromRgb(5, 150, 105),
                StrokeThickness = 2,
                LineStyle = LineStyle.Dash,
                MarkerType = MarkerType.Square,
                MarkerSize = 4,
                MarkerFill = OxyColor.FromRgb(5, 150, 105),
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1.5
            };
            for (int i = 0; i < curveOverall.Count; i++)
                series2.Points.Add(new DataPoint(i, (double)curveOverall[i]));

            model.Series.Add(series1);
            model.Series.Add(series2);

            PlotModel = model;
        }

        public class EquityPoint
        {
            public int Index { get; set; }
            public decimal Balance { get; set; }
        }
    }
}