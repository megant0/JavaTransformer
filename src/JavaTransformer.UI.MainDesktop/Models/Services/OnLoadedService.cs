using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Services
{
    public interface OnLoadedService
    {
        public void Loaded(object sender, RoutedEventArgs args);
    }
}
