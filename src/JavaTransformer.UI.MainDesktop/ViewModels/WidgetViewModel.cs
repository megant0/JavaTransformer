using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.ViewModels
{
    public class WidgetViewModel : ViewModelBase
    {
        private string _title;
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private bool _isDragging;

        public string Title
        {
            get => _title;
            set => this.SetProperty(ref _title, value);
        }

        public double X
        {
            get => _x;
            set => this.SetProperty(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => this.SetProperty(ref _y, value);
        }

        public double Width
        {
            get => _width;
            set => this.SetProperty(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => this.SetProperty(ref _height, value);
        }

        public bool IsDragging
        {
            get => _isDragging;
            set => this.SetProperty(ref _isDragging, value);
        }
    }
}
