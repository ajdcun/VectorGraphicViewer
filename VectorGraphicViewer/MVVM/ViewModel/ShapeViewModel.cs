using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicViewer.MVVM.ViewModel
{
    public class ShapeViewModel : BaseViewModel
    {
        private PathGeometry _shapeGeometry;
        public PathGeometry ShapeGeometry
        {
            get { return _shapeGeometry; }
            set
            {
                _shapeGeometry = value;
                OnPropertyChanged(nameof(ShapeGeometry));
            }
        }

        private PathFigure _shapeFigure;
        public PathFigure ShapeFigure
        {
            get { return _shapeFigure; }
            set
            {
                _shapeFigure = value;
                OnPropertyChanged(nameof(ShapeFigure));
            }
        }

        private ObservableCollection<double> _shapeRangesX;
        public ObservableCollection<double> ShapeRangesX
        {
            get { return _shapeRangesX; }
            set
            {
                _shapeRangesX = value;
                OnPropertyChanged(nameof(ShapeRangesX));
            }
        }

        private ObservableCollection<double> _shapeRangesY;
        public ObservableCollection<double> ShapeRangesY
        {
            get { return _shapeRangesY; }
            set
            {
                _shapeRangesY = value;
                OnPropertyChanged(nameof(ShapeRangesY));
            }
        }

        private Brush _shapeColor;
        public Brush ShapeColor
        {
            get { return _shapeColor; }
            set
            {
                _shapeColor = value;
                OnPropertyChanged(nameof(ShapeColor));
            }
        }

        public ShapeViewModel()
        {
            ShapeGeometry = new PathGeometry();
            ShapeGeometry.FillRule = FillRule.Nonzero;
            ShapeFigure = new PathFigure();
            ShapeRangesX = new ObservableCollection<double>();
            ShapeRangesY = new ObservableCollection<double>();
        }

        public Point GetPoints(string pointString)
        {
            var coordinates = pointString.Split(';');
            if (coordinates.Length != 2)
                return new Point();

            var x = double.Parse(coordinates[0]);
            var y = double.Parse(coordinates[1]);

            return new Point(x, y);
        }

        public Brush GetBrush(string colorString)
        {
            var colorParts = colorString.Split(';');
            byte alpha = byte.Parse(colorParts[0]);
            byte red = byte.Parse(colorParts[1]);
            byte green = byte.Parse(colorParts[2]);
            byte blue = byte.Parse(colorParts[3]);

            return new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
        }
    }
}
