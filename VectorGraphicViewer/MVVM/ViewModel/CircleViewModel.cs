using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using VectorGraphicViewer.MVVM.Model;

namespace VectorGraphicViewer.MVVM.ViewModel
{
    public class CircleViewModel : ShapeViewModel
    {
        private ObservableCollection<CircleModel> _circleModels;
        public ObservableCollection<CircleModel> CircleModels
        {
            get { return _circleModels; }
            set
            {
                _circleModels = value;
                OnPropertyChanged(nameof(CircleModels));
            }
        }

        private CircleModel _circleModel;
        public CircleModel CircleModel
        {
            get { return _circleModel; }
            set
            {
                _circleModel = value;
                OnPropertyChanged(nameof(CircleModel));
            }
        }

        public CircleViewModel()
        {
            CircleModels = new ObservableCollection<CircleModel>();
        }

        public void AddCircleModel(JObject circleObject)
        {
            CircleModel = new CircleModel();

            CircleModel.Type = circleObject.Value<string>("type");
            CircleModel.Color = GetBrush(circleObject.Value<string>("color"));
            CircleModel.Center = GetPoints(circleObject.Value<string>("center"));
            CircleModel.Radius = circleObject.Value<double>("radius");
            CircleModel.Filled = circleObject.Value<bool>("filled");

            CircleModels.Add(CircleModel);
        }

        public void SetShapeRanges()
        {
            ShapeRangesX.Add(Math.Abs(CircleModel.Center.X) + CircleModel.Radius);
            ShapeRangesY.Add(Math.Abs(CircleModel.Center.Y) + CircleModel.Radius);
        }

        public void CreateCircleGeometry(double scaleFactor)
        {
            ShapeGeometry.Clear();

            foreach (CircleModel circleModel in CircleModels)
            {
                double centerPointX = circleModel.Center.X * scaleFactor;
                double centerPointY = circleModel.Center.Y * scaleFactor;
                double radius = circleModel.Radius * scaleFactor;

                ShapeFigure = new PathFigure
                {
                    StartPoint = new Point(centerPointX + radius, centerPointY),
                    Segments =
                    {
                        new ArcSegment(new Point(centerPointX - radius, centerPointY), new Size(radius, radius), 0, false, SweepDirection.Clockwise, true),
                        new ArcSegment(new Point(centerPointX + radius, centerPointY), new Size(radius, radius), 0, false, SweepDirection.Clockwise, true)
                    },
                    IsClosed = true,
                    IsFilled = circleModel.Filled
                };

                ShapeGeometry.Figures.Add(ShapeFigure);
                ShapeColor = circleModel.Color;
            }
        }
    }
}
