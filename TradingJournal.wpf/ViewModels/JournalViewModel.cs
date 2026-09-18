using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using TradingJournal.core.Interfaces;
using TradingJournal.core.Models;
using TradingJournal.core.Interfaces;
using TradingJournal.core.Models;
using TradingJournal.wpf.ViewModels;
using TradingJournal.wpf.Helpers;

namespace TradingJournal.Wpf.ViewModels
{
    public class JournalViewModel : BaseViewModel
    {
        private readonly ITradeRepository _repository;

        // ── Trades-lista (ObservableCollection uppdaterar UI automatiskt) ──
        public ObservableCollection<Trade> Trades { get; set; } = new();

        // ── Formulärfält för ny trade ──
        private string _asset = string.Empty;
        public string Asset
        {
            get => _asset;
            set => SetProperty(ref _asset, value);
        }

        private bool _isLong = true;
        public bool IsLong
        {
            get => _isLong;
            set => SetProperty(ref _isLong, value);
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

        private decimal _exitPrice;
        public decimal ExitPrice
        {
            get => _exitPrice;
            set => SetProperty(ref _exitPrice, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private decimal _deposit;
        public decimal Deposit
        {
            get => _deposit;
            set => SetProperty(ref _deposit, value);
        }

        private decimal _withdraw;
        public decimal Withdraw
        {
            get => _withdraw;
            set => SetProperty(ref _withdraw, value);
        }

        private DateTime _closeDate = DateTime.Today;
        public DateTime CloseDate
        {
            get => _closeDate;
            set => SetProperty(ref _closeDate, value);
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        // ── Felmeddelande ──
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        // ── Vald trade i listan ──
        private Trade? _selectedTrade;
        public Trade? SelectedTrade
        {
            get => _selectedTrade;
            set => SetProperty(ref _selectedTrade, value);
        }

        // ── Kommandon ──
        public ICommand AddTradeCommand { get; }
        public ICommand DeleteTradeCommand { get; }
        public ICommand ClearFormCommand { get; }

        public JournalViewModel()
        {
            // Hämta repository via DI
            _repository = (ITradeRepository)App.ServiceProvider
                .GetService(typeof(ITradeRepository))!;

            AddTradeCommand = new RelayCommand(_ => AddTrade(), _ => CanAddTrade());
            DeleteTradeCommand = new RelayCommand(_ => DeleteTrade(), _ => SelectedTrade != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            LoadTrades();
        }

        private void LoadTrades()
        {
            Trades.Clear();
            foreach (var trade in _repository.GetAll())
                Trades.Add(trade);
        }

        private bool CanAddTrade()
        {
            return !string.IsNullOrWhiteSpace(Asset)
                && Entry > 0
                && StopLoss > 0
                && TakeProfit > 0
                && ExitPrice > 0
                && Amount > 0
                && StopLoss != Entry;
        }

        private void AddTrade()
        {
            ErrorMessage = string.Empty;

            try
            {
                Trade trade = IsLong
                    ? new LongTrade()
                    : new ShortTrade();

                trade.Asset = Asset;
                trade.TradeType = IsLong ? "Long" : "Short";
                trade.CloseDate = CloseDate;
                trade.Entry = Entry;
                trade.StopLoss = StopLoss;
                trade.TakeProfit = TakeProfit;
                trade.ExitPrice = ExitPrice;
                trade.Amount = Amount;
                trade.Deposit = Deposit;
                trade.Withdraw = Withdraw;
                trade.Notes = Notes;

                _repository.Add(trade);
                Trades.Insert(0, trade);
                ClearForm();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fel: {ex.Message}";
            }
        }

        private void DeleteTrade()
        {
            if (SelectedTrade == null) return;

            _repository.Delete(SelectedTrade.Id);
            Trades.Remove(SelectedTrade);
            SelectedTrade = null;
        }

        private void ClearForm()
        {
            Asset = string.Empty;
            IsLong = true;
            Entry = 0;
            StopLoss = 0;
            TakeProfit = 0;
            ExitPrice = 0;
            Amount = 0;
            Deposit = 0;
            Withdraw = 0;
            CloseDate = DateTime.Today;
            Notes = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}