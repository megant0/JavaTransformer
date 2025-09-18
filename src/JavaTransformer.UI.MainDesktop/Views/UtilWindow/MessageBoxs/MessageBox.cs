using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs
{
    public class MessageBox
    {
        public static void Show(object message, [CallerMemberName] string title = "")
        {
            MessageBoxWindow box = new MessageBoxWindow();
            box.ShowMessage(message.ToString() ?? message.GetType().ToString(), title.ToString(), null);
        }

        public static void Show(Window parent, object message, [CallerMemberName] string title = "")
        {
            MessageBoxWindow box = new MessageBoxWindow();
            box.ShowMessage(message.ToString() ?? message.GetType().ToString(), title, parent);
        }
    }
}
