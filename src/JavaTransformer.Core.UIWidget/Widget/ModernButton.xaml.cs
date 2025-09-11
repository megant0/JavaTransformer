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
using System.Windows.Media.Animation;
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

        public static DependencyProperty EnterAnimationProperty 
            = ModernUserControl.createProperty<float, ModernButton>("EnterAnimation");

        public static DependencyProperty LeaveAnimationProperty
            = ModernUserControl.createProperty<float, ModernButton>("LeaveAnimation");

        public static DependencyProperty HoverForegroundProperty
            = ModernUserControl.createProperty<Brush, ModernButton>("HoverForeground");
       
        public static DependencyProperty OtherColorProperty
             = ModernUserControl.createProperty<Brush, ModernButton>("OtherColor");

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

        public Brush HoverForeground
        {
            get => (Brush)GetValue(HoverForegroundProperty);
            set => SetValue(HoverForegroundProperty, value);
        }

        public Brush OtherColor
        {
            get => (Brush)GetValue(OtherColorProperty);
            set => SetValue(OtherColorProperty, value);
        }

        public float EnterAnimation
        {
            get => (float)GetValue(EnterAnimationProperty);
            set => SetValue(EnterAnimationProperty, value);
        }

        public float LeaveAnimation
        {
            get => (float)GetValue(LeaveAnimationProperty);
            set => SetValue(LeaveAnimationProperty, value);
        }


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
            if (Color is SolidColorBrush colorBrush)
            {
                var newBrush = new SolidColorBrush(_CurrentBorder.Background is SolidColorBrush currentBrush
                    ? currentBrush.Color
                    : colorBrush.Color);

                _CurrentBorder.Background = newBrush;

                var animation = new ColorAnimation
                {
                    To = colorBrush.Color,
                    Duration = TimeSpan.FromSeconds(LeaveAnimation),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                _CurrentBorder.Background = Color;
            }

            Foreground = OtherColor;
        }

        private void ModernButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Hover is SolidColorBrush hoverBrush)
            {
                var newBrush = new SolidColorBrush(_CurrentBorder.Background is SolidColorBrush currentBrush
                    ? currentBrush.Color
                    : hoverBrush.Color);

                _CurrentBorder.Background = newBrush;

                var animation = new ColorAnimation
                {
                    To = hoverBrush.Color,
                    Duration = TimeSpan.FromSeconds(EnterAnimation),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                _CurrentBorder.Background = Hover;
            }

            Foreground = HoverForeground;
        }
    }
}
