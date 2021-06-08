using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vlak
{
    class Hranice
    {
        private Rectangle border;
        public static Grid MRIZ;
        public static List<Rectangle> wall { get; private set; } = new List<Rectangle>() ;
        private ImageBrush HraniceTex = new ImageBrush();
        public Hranice(int left, int down)
        {
            border = new Rectangle();
            border.Width = 50;
            border.Height = 50;
            border.Margin = new Thickness(left, down, 0, 0);
            border.VerticalAlignment = VerticalAlignment.Top;
            border.HorizontalAlignment = HorizontalAlignment.Left;         
            MRIZ.Children.Add(border);                                                 
            HraniceTex.ImageSource = new BitmapImage(new Uri("../../Resources/Hranice.png", UriKind.Relative));
            border.Fill = HraniceTex;
            Hranice.wall.Add(border);
        }
    }
}
