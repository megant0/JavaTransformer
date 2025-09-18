using Avalonia.Controls;
using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Views.UtilWindow.RenameFileWindows
{
    public class RenameInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
    }

    public class RenameFileDialog : IDisposable
    {
        private string _path;
        private RenameFileWindow _window;

        public EventHandler<RenameInfo> OnFileNameConfirmed;

        public RenameFileDialog(string path)
        {
            _path = path;
            _window = new RenameFileWindow();
        }

        private string? GetName()
        {
            return FileTask.RequestName(_path);
        }

        public void Show(Window parent)
        {

            string? _name = GetName();

            if (_name == null) return;

            _window.OnFileNameConfirmed += Window_OnFileNameConfirmed;
            _window.ShowDialog(parent, _name);
        }

        private void Window_OnFileNameConfirmed(object? sender, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return;

            string? directory = Path.GetDirectoryName(_path);
            if (string.IsNullOrEmpty(directory)) return;

            string newFullPath = Path.Combine(directory, newFileName);

            OnFileNameConfirmed?.Invoke(this, new RenameInfo() 
            {
               Name = newFileName,
               Path = newFullPath
            });
        }

        public void Close()
        {
            _window.OnFileNameConfirmed -= Window_OnFileNameConfirmed;
            _window.Close();
        }

        public string GetPath()
        {
            return _path;
        }

        public void Dispose()
        {
            _window.Close();
            OnFileNameConfirmed = null;
        }
    }
}
