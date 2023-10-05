using System.Windows;

namespace VectorGraphicViewer.MVVM.Model
{
    public class TriangleModel : ShapeModel
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public bool Filled { get; set; }
    }

}
