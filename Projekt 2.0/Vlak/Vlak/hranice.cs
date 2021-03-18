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
    class hranice
    {
        private Rectangle border;
        public static Grid MRIZ;
        public static List<Rectangle> wall = new List<Rectangle>();
        public hranice(int left, int down)
        {
            border = new Rectangle();
            border.Width = 50;
            border.Height = 50;
            border.Margin = new Thickness(left, down, 0, 0);
            border.VerticalAlignment = VerticalAlignment.Top;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            MRIZ.Children.Add(border);
            ImageBrush HraniceTex = new ImageBrush();
            HraniceTex.ImageSource = new BitmapImage(new Uri("../../Resources/hranice.png", UriKind.Relative));
            border.Fill = HraniceTex;
            hranice.wall.Add(border);
        }
    }
}
