using JavaTransformer.Core.UIWidget.Api;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

        private CustomSetupStyle setupStyleController;

        public MStyle()
        {
           
        }

        public MStyle(UIElement element, CustomSetupStyle custom = null) 
        {
            if (element == null) throw new NullReferenceException("UIElement is null");
            _element = element;

            if (custom != null)
                setupStyleController = custom;
            else setupStyleController = new DefaultSetupStyle(element);

            Initialize();
        }

        public void InjectCustomSetupStyle(CustomSetupStyle b)
        {
            setupStyleController = b;
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

        public virtual void setBackground(LColor color)
        {
            setupStyleController.setBackground(color);
        }

        public virtual void setForeground(LColor color)
        {
            setupStyleController.setForeground(color);
        }

        public virtual void setThickness(Thickness th)
        {
            setupStyleController.setThickness(th);
        }

        public virtual void setBorder(LColor color)
        {
            setupStyleController.setBorder(color);
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
            if (_element == null) return;

            _element.MouseDown  -= _element_MouseDown;
            _element.MouseEnter -= _element_MouseEnter;
            _element.MouseLeave -= _element_MouseLeave;
        }
    }
}
