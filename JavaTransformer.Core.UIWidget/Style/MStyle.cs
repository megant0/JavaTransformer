using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JavaTransformer.Core.UIWidget.Style
{
    public class MStyle : IDisposable
    {
        protected UIElement _element;

        public LColor Background;
        public LColor Foreground;
        public LColor Border;

        public LColor ClickBackground;
        public LColor ClickForeground;
        public LColor ClickBorder;

        public LColor HoverBackground;
        public LColor HoverForeground;
        public LColor HoverBorder;

        public Thickness Thickness;
        public Thickness HoverThickness;
        public Thickness ClickThickness;

        public MStyle(UIElement element) 
        {
            if (element == null) throw new NullReferenceException("UIElement is null");
            _element = element;

            Initialize();
        }

        public MStyle(UIElement s, MStyle copy)
            : this(s)
        {
            CopyStyle(copy);
        }

        public MStyle(UIElement element, 
            LColor background, 
            LColor foreground, 
            LColor border, 
            LColor clickBackground, 
            LColor clickForeground, 
            LColor clickBorder, 
            LColor hoverBackground, 
            LColor hoverForeground, 
            LColor hoverBorder, 
            Thickness thickness, 
            Thickness hoverThickness, 
            Thickness clickThickness) 
            : this(element)
        {
            Background = background;
            Foreground = foreground;
            Border = border;
            ClickBackground = clickBackground;
            ClickForeground = clickForeground;
            ClickBorder = clickBorder;
            HoverBackground = hoverBackground;
            HoverForeground = hoverForeground;
            HoverBorder = hoverBorder;
            Thickness = thickness;
            HoverThickness = hoverThickness;
            ClickThickness = clickThickness;
        }

        protected void Initialize()
        {
            _element.MouseDown += _element_MouseDown;
            _element.MouseEnter += _element_MouseEnter;
            _element.MouseLeave += _element_MouseLeave;
        }

        private void setBackground(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetBrushColor(_element, color);
        }

        private void setForeground(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetOtherColor(_element, color);
        }

        private void setThickness(Thickness th)
        {
            WConvertor32.SetBorderSize(_element, th);
        }

        private void setBorder(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetBorderColor(_element, color);
        }

        private void _element_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            setBackground(Background);
            setForeground(Foreground);
            setThickness(Thickness);
            setBorder(Border);
        }

        private void _element_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            setBackground(HoverBackground);
            setForeground(HoverForeground);
            setThickness(HoverThickness);
            setBorder(HoverBorder);
        }

        private void _element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            setBackground(ClickBackground);
            setForeground(ClickForeground);
            setThickness(ClickThickness);
            setBorder(ClickBorder);
        }

        public void CopyStyle(MStyle style)
        {
            if (style == null)
                throw new ArgumentNullException(nameof(style), "Style cannot be null");

            Background = style.Background;
            Foreground = style.Foreground;
            Border = style.Border;
            Thickness = style.Thickness;

            HoverBackground = style.HoverBackground;
            HoverForeground = style.HoverForeground;
            HoverBorder = style.HoverBorder;
            HoverThickness = style.HoverThickness;

            ClickBackground = style.ClickBackground;
            ClickForeground = style.ClickForeground;
            ClickBorder = style.ClickBorder;
            ClickThickness = style.ClickThickness;

            ApplyCurrentStyle();
        }

        public void ApplyCurrentStyle()
        {
            setBackground(Background);
            setForeground(Foreground);
            setBorder(Border);
            setThickness(Thickness);
        }

        public void Dispose()
        {
            _element.MouseDown  -= _element_MouseDown;
            _element.MouseEnter -= _element_MouseEnter;
            _element.MouseLeave -= _element_MouseLeave;
        }
    }
}
