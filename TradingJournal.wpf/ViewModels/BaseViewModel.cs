using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TradingJournal.wpf.ViewModels
{
    // This abstract class serves as a base for all ViewModel classes in the application, providing property change notification functionality.
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // This method sets the property value and raises the PropertyChanged event if the value has changed.
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
