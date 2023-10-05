using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VectorGraphicViewer.Services;

namespace VectorGraphicViewer.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private PathGeometry _coordinateGeometry;
        public PathGeometry CoordinateGeometry
        {
            get { return _coordinateGeometry; }
            set
            {
                _coordinateGeometry = value;
                OnPropertyChanged(nameof(CoordinateGeometry));
            }
        }

        private LineViewModel _lineViewModel;
        public LineViewModel LineViewModel
        {
            get => _lineViewModel;
            set
            {
                _lineViewModel = value;
                OnPropertyChanged(nameof(LineViewModel));
            }
        }

        private CircleViewModel _circleViewModel;
        public CircleViewModel CircleViewModel
        {
            get => _circleViewModel;
            set
            {
                _circleViewModel = value;
                OnPropertyChanged(nameof(CircleViewModel));
            }
        }

        private TriangleViewModel _triangleViewModel;
        public TriangleViewModel TriangleViewModel
        {
            get => _triangleViewModel;
            set
            {
                _triangleViewModel = value;
                OnPropertyChanged(nameof(TriangleViewModel));
            }
        }

        private IDataService _data;
        public IDataService Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private double _windowWidth;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));
            }
        }

        private double _windowHeight;
        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                OnPropertyChanged(nameof(WindowHeight));
            }
        }

        private double _scaleFactor;
        public double ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                OnPropertyChanged(nameof(ScaleFactor));
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

        private string _fileName = "file.json";
        private double _margin = 10;
        private double _upperMargin = SystemParameters.CaptionHeight;

        public MainViewModel(IDataService dataService, LineViewModel lineViewModel, CircleViewModel circleViewModel, TriangleViewModel triangleViewModel)
        {
            Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
            CoordinateGeometry = new PathGeometry();

            Data = dataService;
            LineViewModel = lineViewModel;
            CircleViewModel = circleViewModel;
            TriangleViewModel = triangleViewModel;

            dataService.SetFilePath(_fileName);
            JArray shapeArray = dataService.GetJsonData();

            foreach (JObject shapeObject in shapeArray)
            { 
                string shapeType = shapeObject.Value<string>("type");

                switch (shapeType)
                {
                    case "line":
                        LineViewModel.AddLineModel(shapeObject);
                        LineViewModel.SetShapeRanges();
                        break;

                    case "circle":
                        CircleViewModel.AddCircleModel(shapeObject);
                        CircleViewModel.SetShapeRanges();
                        break;

                    case "triangle":
                        TriangleViewModel.AddTriangleModel(shapeObject);
                        TriangleViewModel.SetShapeRanges();
                        break;

                    default:
                        continue;
                }
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetWindowSizes(e);
            SetScaleFactor();
            CreateCoordinateGeometry();
            CreateGeometries(ScaleFactor);
        }

        public void SetWindowSizes(SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
        }

        public void SetScaleFactor()
        {
            double maxRangeX = LineViewModel.ShapeRangesX
                .Concat(CircleViewModel.ShapeRangesX)
                .Concat(TriangleViewModel.ShapeRangesX)
                .ToList()
                .Max();

            double maxRangeY = LineViewModel.ShapeRangesY
                .Concat(CircleViewModel.ShapeRangesY)
                .Concat(TriangleViewModel.ShapeRangesY)
                .ToList()
                .Max();

            double desiredGeometryWidth = (maxRangeX + _margin) * 2;
            double desiredGeometryHeigth = (maxRangeY + _margin) * 2 + _upperMargin;

            ScaleFactor = Math.Min(WindowWidth / desiredGeometryWidth, WindowHeight / desiredGeometryHeigth);
        }

        public void CreateCoordinateGeometry()
        {
            CoordinateGeometry.Clear();

            double halfWidth = (WindowWidth / 2);
            double halfHeight = (WindowHeight / 2);

            var coordinateXFigure = new PathFigure
            {
                StartPoint = new Point(-halfWidth, 0),
                Segments = { new LineSegment(new Point(halfWidth, 0), true) }
            };

            var coordinateYFigure = new PathFigure
            {
                StartPoint = new Point(0, -halfHeight),
                Segments = { new LineSegment(new Point(0, halfHeight), true) }
            };

            CoordinateGeometry.Figures.Add(coordinateXFigure);
            CoordinateGeometry.Figures.Add(coordinateYFigure);
        }

        public void CreateGeometries(double scaleFactor)
        {
            LineViewModel.CreateLineGeometry(ScaleFactor);
            CircleViewModel.CreateCircleGeometry(ScaleFactor);
            TriangleViewModel.CreateTriangleGeometry(ScaleFactor);
        }
    }
}
