using JavaTransformer.Core.UIWidget.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JavaTransformer.Core.UIWidget.Api
{
    public interface CustomSetupStyle
    {
        void setBackground(LColor color);

        void setForeground(LColor color);

        void setThickness(Thickness th);

        void setBorder(LColor color);
    }
}
