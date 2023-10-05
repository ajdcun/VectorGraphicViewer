using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VectorGraphicViewer.MVVM.Model;

namespace VectorGraphicViewer.MVVM.ViewModel
{
    public class TriangleViewModel : ShapeViewModel
    {
        private ObservableCollection<TriangleModel> _triangleModels;
        public ObservableCollection<TriangleModel> TriangleModels
        {
            get { return _triangleModels; }
            set
            {
                _triangleModels = value;
                OnPropertyChanged(nameof(TriangleModels));
            }
        }

        private TriangleModel _triangleModel;
        public TriangleModel TriangleModel
        {
            get { return _triangleModel; }
            set
            {
                _triangleModel = value;
                OnPropertyChanged(nameof(TriangleModel));
            }
        }

        public TriangleViewModel()
        {
            TriangleModels = new ObservableCollection<TriangleModel>();
        }

        public void AddTriangleModel(JObject triangleObject)
        {
            TriangleModel = new TriangleModel();

            TriangleModel.Type = triangleObject.Value<string>("type");
            TriangleModel.Color = GetBrush(triangleObject.Value<string>("color"));
            TriangleModel.A = GetPoints(triangleObject.Value<string>("a"));
            TriangleModel.B = GetPoints(triangleObject.Value<string>("b"));
            TriangleModel.C = GetPoints(triangleObject.Value<string>("c"));
            TriangleModel.Filled = triangleObject.Value<bool>("filled");

            TriangleModels.Add(TriangleModel);
        }

        public void SetShapeRanges()
        {
            ObservableCollection<double> coordinatesX = new ObservableCollection<double>
            {
                Math.Abs(TriangleModel.A.X),
                Math.Abs(TriangleModel.B.X),
                Math.Abs(TriangleModel.C.X)
            };

            ObservableCollection<double> coordinatesY = new ObservableCollection<double>
            {
                Math.Abs(TriangleModel.A.Y),
                Math.Abs(TriangleModel.B.Y),
                Math.Abs(TriangleModel.C.Y)
            };

            ShapeRangesX.Add(coordinatesX.Max());
            ShapeRangesY.Add(coordinatesY.Max());
        }

        public void CreateTriangleGeometry(double scaleFactor)
        {
            ShapeGeometry.Clear();

            foreach (TriangleModel triangleModel in TriangleModels)
            {
                double pointAX = triangleModel.A.X * scaleFactor;
                double pointAY = triangleModel.A.Y * scaleFactor;
                double pointBX = triangleModel.B.X * scaleFactor;
                double pointBY = triangleModel.B.Y * scaleFactor;
                double pointCX = triangleModel.C.X * scaleFactor;
                double pointCY = triangleModel.C.Y * scaleFactor;

                ShapeFigure = new PathFigure
                {
                    StartPoint = new Point(pointAX, pointAY),
                    Segments =
                    {
                        new LineSegment(new Point(pointBX, pointBY), true),
                        new LineSegment(new Point(pointCX, pointCY), true)
                    },
                    IsClosed = true,
                    IsFilled = triangleModel.Filled
                };

                ShapeGeometry.Figures.Add(ShapeFigure);
                ShapeColor = triangleModel.Color;
            }
        }
    }
}
