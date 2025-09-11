using JavaTransformer.Core.UIWidget.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace JavaTransformer.Core.UIWidget.Api
{
    public abstract class ModernUserControl : UserControl
    {
        public static DependencyProperty createProperty<T1, T2>([CallerMemberName] string prop = null) 
        => DependencyProperty.Register(prop, typeof(T1), typeof(T2), new PropertyMetadata(null));
        

        public static DependencyProperty createProperty<T1, T2>([CallerMemberName] string prop = null, string meta = null)
        => DependencyProperty.Register(prop, typeof(T1), typeof(T2), new PropertyMetadata(meta));

        public abstract bool injectStyle(MStyle style);
    }
}
