using System.Windows;

namespace VectorGraphicViewer.MVVM.Model
{
    public class CircleModel : ShapeModel
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
        public bool Filled { get; set; }
    }

}
