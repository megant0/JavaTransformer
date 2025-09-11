using JavaTransformer.Core.UIWidget.Api;
using JavaTransformer.Core.UIWidget.Style;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace JavaTransformer.Core.UIWidget.Widget
{
    public partial class ModernToggleButton : UserControl
    {
        public bool enabled = false;

        public static DependencyProperty TextProperty
            = ModernUserControl.createProperty<string, ModernToggleButton>("Text");

        public static DependencyProperty ColorProperty
            = ModernUserControl.createProperty<Brush, ModernToggleButton>("Color");

        public static DependencyProperty HoverProperty
            = ModernUserControl.createProperty<Brush, ModernToggleButton>("Hover");

        public static DependencyProperty ResultProperty
            = ModernUserControl.createProperty<Brush, ModernToggleButton>("Result");

        public static DependencyProperty CornerRadiusProperty
            = ModernUserControl.createProperty<CornerRadius, ModernToggleButton>("CornerRadius");

        public static DependencyProperty HoverBorderProperty
            = ModernUserControl.createProperty<Brush, ModernToggleButton>("HoverBorder");

        public static DependencyProperty BorderProperty
             = ModernUserControl.createProperty<Brush, ModernToggleButton>("Border");

        public static DependencyProperty ThicknessProperty
            = ModernUserControl.createProperty<Thickness, ModernToggleButton>("Thickness");

        public static DependencyProperty EnterAnimationProperty
            = ModernUserControl.createProperty<float, ModernToggleButton>("EnterAnimation");

        public static DependencyProperty LeaveAnimationProperty
            = ModernUserControl.createProperty<float, ModernToggleButton>("LeaveAnimation");

        public static DependencyProperty HoverForegroundProperty
            = ModernUserControl.createProperty<Brush, ModernToggleButton>("HoverForeground");

        public static DependencyProperty OtherColorProperty
             = ModernUserControl.createProperty<Brush, ModernToggleButton>("OtherColor");

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

        public Brush Result
        {
            get => (Brush)GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
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
            set => SetValue(HoverBorderProperty, value);
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

        public ModernToggleButton()
        {
            InitializeComponent();
            MouseEnter += ModernToggleButton_MouseEnter;
            MouseLeave += ModernToggleButton_MouseLeave;
            MouseDown += ModernToggleButton_MouseDown;
        }

        private void ModernToggleButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            enabled = !enabled;
            UpdateAppearance();
        }

        private void ModernToggleButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enabled)
            {
                AnimateBackground(Hover, EnterAnimation);
                if (HoverForeground != null)
                    Foreground = HoverForeground;
            }
        }

        private void ModernToggleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (enabled)
            {
                AnimateBackground(Result, LeaveAnimation);
                if (OtherColor != null)
                    Foreground = OtherColor;
            }
            else
            {
                AnimateBackground(Color, LeaveAnimation);
                if (OtherColor != null)
                    Foreground = OtherColor;
            }
        }

        private void UpdateAppearance()
        {
            if (enabled)
            {
                AnimateBackground(Result, EnterAnimation);
                if (HoverForeground != null)
                    Foreground = HoverForeground;
            }
            else
            {
                AnimateBackground(Color, LeaveAnimation);
                if (OtherColor != null)
                    Foreground = OtherColor;
            }
        }

        private void AnimateBackground(Brush targetBrush, float duration)
        {
            if (targetBrush is SolidColorBrush targetSolidBrush)
            {
                var newBrush = new SolidColorBrush(_CurrentBorder.Background is SolidColorBrush currentBrush
                    ? currentBrush.Color
                    : targetSolidBrush.Color);

                _CurrentBorder.Background = newBrush;

                var animation = new ColorAnimation
                {
                    To = targetSolidBrush.Color,
                    Duration = TimeSpan.FromSeconds(duration),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                _CurrentBorder.Background = targetBrush;
            }
        }
    }
}