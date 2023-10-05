using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VectorGraphicViewer.MVVM.Model;

namespace VectorGraphicViewer.MVVM.ViewModel
{
    public class LineViewModel : ShapeViewModel
    {
        private ObservableCollection<LineModel> _lineModels;
        public ObservableCollection<LineModel> LineModels
        {
            get { return _lineModels; }
            set
            {
                _lineModels = value;
                OnPropertyChanged(nameof(LineModels));
            }
        }

        private LineModel _lineModel;
        public LineModel LineModel
        {
            get { return _lineModel; }
            set
            {
                _lineModel = value;
                OnPropertyChanged(nameof(LineModel));
            }
        }

        public LineViewModel()
        {
            LineModels = new ObservableCollection<LineModel>();
        }

        public void AddLineModel(JObject lineObject)
        {
            LineModel = new LineModel();

            LineModel.Type = lineObject.Value<string>("type");
            LineModel.Color = GetBrush(lineObject.Value<string>("color"));
            LineModel.A = GetPoints(lineObject.Value<string>("a"));
            LineModel.B = GetPoints(lineObject.Value<string>("b"));

            LineModels.Add(LineModel);
        }

        public void SetShapeRanges()
        {
            ObservableCollection<double> coordinatesX = new ObservableCollection<double>
            {
                Math.Abs(LineModel.A.X),
                Math.Abs(LineModel.B.X)
            };

            ObservableCollection<double> coordinatesY = new ObservableCollection<double>
            {
                Math.Abs(LineModel.A.Y),
                Math.Abs(LineModel.B.Y)
            };

            ShapeRangesX.Add(coordinatesX.Max());
            ShapeRangesY.Add(coordinatesY.Max());
        }

        public void CreateLineGeometry(double scaleFactor)
        {
            ShapeGeometry.Clear();

            foreach (LineModel lineModel in LineModels)
            {
                double pointAX = lineModel.A.X * scaleFactor;
                double pointAY = lineModel.A.Y * scaleFactor;
                double pointBX = lineModel.B.X * scaleFactor;
                double pointBY = lineModel.B.Y * scaleFactor;

                ShapeFigure = new PathFigure
                {
                    StartPoint = new Point(pointAX, pointAY),
                    Segments =
                    {
                        new LineSegment(new Point(pointBX, pointBY), true)
                    }
                };

                ShapeGeometry.Figures.Add(ShapeFigure);
                ShapeColor = lineModel.Color;
            }
        }
    }
}
