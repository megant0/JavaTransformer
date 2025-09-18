using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Views.UtilWindow.RenameWindows
{
    public class RenameDialog
    {
        private string _currentName;
        private RenameWindow _window;

        public EventHandler<string> OnNameConfirmed;

        public RenameDialog(string currentName)
        {
            _currentName = currentName;
            _window = new RenameWindow();
        }

        public void Show(Window parent) 
        {
            _window.OnNameConfirmed += _window_OnNameConfirmed;
            _window.ShowDialog(parent, _currentName);
        }

        private void _window_OnNameConfirmed(object? sender, string e)
        {
            OnNameConfirmed?.Invoke(sender, e);
        }

        public string GetCurrentName()
        {
            return _currentName;
        }
    }
}
