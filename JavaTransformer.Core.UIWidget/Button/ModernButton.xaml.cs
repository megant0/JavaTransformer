using JavaTransformer.Core.UIWidget.Api;
using JavaTransformer.Core.UIWidget.Style;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JavaTransformer.Core.UIWidget
{
    public partial class ModernButton : UserControl
    {
        private MStyle currentStyle = null;

        public ModernButton(MStyle style = null)
        {
            InitializeComponent();
            currentStyle = style;

            if(style != null) 
            {
                currentStyle = new MStyle(this, style);
            }
            else currentStyle = new MStyle(this);
        }

        private void CurrentBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void CurrentBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void CurrentBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
         
        }

        public static readonly DependencyProperty TextProperty = ModernUserControl.createProperty<string, ModernButton>("Text", "Button");
        public static readonly DependencyProperty CornerRadiusProperty = ModernUserControl.createProperty<float, ModernButton>("CornerRadius");

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}