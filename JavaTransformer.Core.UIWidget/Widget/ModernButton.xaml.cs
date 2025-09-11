using JavaTransformer.Core.UIWidget.Api;
using JavaTransformer.Core.UIWidget.Extensions;
using JavaTransformer.Core.UIWidget.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JavaTransformer.Core.UIWidget.Widget
{
    public partial class ModernButton : UserControl, CustomSetupStyle
    {

        public static DependencyProperty TextProperty
            = ModernUserControl.createProperty<string, ModernButton>("Text");

        public static DependencyProperty ColorProperty
            = ModernUserControl.createProperty<Brush, ModernButton>("Color");

        public static DependencyProperty HoverProperty
            = ModernUserControl.createProperty<Brush, ModernButton>("Hover");

        public static DependencyProperty CornerRadiusProperty
            = ModernUserControl.createProperty<CornerRadius, ModernButton>("CornerRadius");

        public static DependencyProperty HoverBorderProperty
            = ModernUserControl.createProperty<Brush, ModernButton>("HoverBorder");

        public static DependencyProperty BorderProperty
             = ModernUserControl.createProperty<Brush, ModernButton>("Border");

        public static DependencyProperty ThicknessProperty
            = ModernUserControl.createProperty<Thickness, ModernButton>("Thickness");

        public void setBackground(LColor color)
        {
            if (color == null) return;

            _CurrentBorder.Background = color.FromSolidColorBrush();
        }

        public void setForeground(LColor color)
        {
            if (color == null) return;

            Foreground = color.FromSolidColorBrush();
        }

        public void setThickness(Thickness th)
        {
            _CurrentBorder.BorderThickness = th;
        }

        public void setBorder(LColor color)
        {
            if (color == null) return;
            _CurrentBorder.BorderBrush = color.FromSolidColorBrush();
        }

        // CLR properties

        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public Brush Hover
        {
            get => (Brush)GetValue(HoverProperty);
            set => SetValue(HoverProperty, value);
        }

        public Brush Border
        {
            get => (Brush)GetValue(BorderProperty);
            set => SetValue(BorderProperty, value);
        }

        public Brush HoverBorder
        {
            get => (Brush)GetValue(HoverBorderProperty);
            set => SetValue(HoverProperty, value);
        }

        public Thickness Thickness
        {
            get => (Thickness)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public ModernButton()
        {
            InitializeComponent();
            MouseEnter += ModernButton_MouseEnter;
            MouseLeave += ModernButton_MouseLeave;
        }

        private void ModernButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _CurrentBorder.Background = Color;
            _CurrentBorder.BorderBrush = Border;
        }

        private void ModernButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _CurrentBorder.Background = Hover;
            _CurrentBorder.BorderBrush = HoverBorder;
        }
    }
}
