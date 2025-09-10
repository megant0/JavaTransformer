using JavaTransformer.Core.UIWidget.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JavaTransformer.Core.UIWidget.Extensions
{
    public static class BrushExtensions
    {
        public static Color? ToColor(this Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
                return solidBrush.Color;

            if (brush is GradientBrush gradientBrush && gradientBrush.GradientStops.Count > 0)
                return gradientBrush.GradientStops[0].Color;

            return null;
        }

        public static LColor ToLColor(this Brush brush)
        {
            var color = brush.ToColor();
            if (color.HasValue)
            {
                return new LColor(color.Value.R, color.Value.G, color.Value.B, color.Value.A);
            }

            return new LColor(0, 0, 0, 0); 
        }
    }
}
