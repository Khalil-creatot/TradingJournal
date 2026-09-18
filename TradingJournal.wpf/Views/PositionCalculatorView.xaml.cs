using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TradingJournal.wpf.Views
{
    public partial class PositionCalculatorView : UserControl
    {
        public PositionCalculatorView()
        {
            InitializeComponent();
        }

        // Markerar all text när du tabbar in i ett fält
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
                tb.SelectAll();
        }

        // Markerar all text när du klickar i ett fält
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }
    }
}