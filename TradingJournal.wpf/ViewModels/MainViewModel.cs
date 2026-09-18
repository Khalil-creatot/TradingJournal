using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.wpf.Helpers;
using System.Windows.Input;

namespace TradingJournal.wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
        public ICommand ShowJournalCommand { get; }
        public ICommand ShowPositionCalculatorCommand { get; }
        public ICommand ShowPerformanceCommand { get; }
       
        public MainViewModel()
        {
            _currentViewModel = new JournalViewModel();

            ShowJournalCommand = new RelayCommand(_ => CurrentViewModel = new JournalViewModel());
            ShowPositionCalculatorCommand = new RelayCommand(_ => CurrentViewModel = new PositionCalculatorViewModel());
            ShowPerformanceCommand = new RelayCommand(_ => CurrentViewModel = new PerformanceViewModel());
        }
    }
       
}
