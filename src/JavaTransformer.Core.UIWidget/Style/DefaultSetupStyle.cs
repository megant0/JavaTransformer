using JavaTransformer.Core.UIWidget.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JavaTransformer.Core.UIWidget.Style
{
    public class DefaultSetupStyle : CustomSetupStyle
    {
        private UIElement _element;

        public DefaultSetupStyle(UIElement element)
        {
            _element = element;
        }

        public virtual void setBackground(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetBrushColor(_element, color);
        }

        public virtual void setForeground(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetOtherColor(_element, color);
        }

        public virtual void setThickness(Thickness th)
        {
            WConvertor32.SetBorderSize(_element, th);
        }

        public virtual void setBorder(LColor color)
        {
            if (color == null) return;

            WConvertor32.SetBorderColor(_element, color);
        }
    }
}
