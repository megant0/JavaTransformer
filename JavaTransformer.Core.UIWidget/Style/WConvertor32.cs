using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JavaTransformer.Core.UIWidget.Style
{
    public static class WConvertor32
    {

        public static void SetForeground(this Control control, Brush _)
           => control.Foreground = _;
        public static Brush GetForeground(this Control control)
            => control.Foreground;

        public static void AddRange(this ComboBox box, object[] objects)
        {
            foreach (var i in objects)
                box.Items.Add(i);
        }

        public static void SetBorderSize(UIElement _control, Thickness _)
        {

            if (_control is Control control)
                control.BorderThickness = _;
            else if (_control is Border border)
                border.BorderThickness = _;
        }
        public static void SetBrushColor(UIElement _control, LColor _)
        {
            if (_control is Control control)
                control.Background = _.FromSolidColorBrush();
            else if (_control is Border border)
                border.Background = _.FromSolidColorBrush();
        }

        public static LColor GetBrushColor(UIElement _control)
        {
            if (_control is Control control)
                return new LColor(control.Background);
            else if (_control is Border border)
                return new LColor(border.Background);

            return null;
        }

        public static LColor GetBorderColor(UIElement _control)
        {
            if (_control is Control control)
                return new LColor(control.BorderBrush);
            else if (_control is Border border)
                return new LColor(border.BorderBrush);

            return null;
        }

        public static LColor GetOtherColor(UIElement _control)
        {
            if (_control is Control control)
                return new LColor(control.Foreground);

            return null;
        }

        public static void SetBorderColor(UIElement _control, LColor _)
        {
            if (_control is Control control)
                control.BorderBrush = _.FromSolidColorBrush();
            else if (_control is Border border)
                border.BorderBrush = _.FromSolidColorBrush();
        }

        public static void SetOtherColor(UIElement _control, LColor _)
        {
            getControl(_control)?.SetForeground(_.FromSolidColorBrush());
        }

        public static Control getControl(UIElement _control)
        {
            return !(_control is Control) ? null : (Control)_control;
        }

        public static Border getBorder(UIElement _control)
        {
            return !(_control is Border) ? null : (Border)_control;
        }
    }
}
