using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
namespace Vlak
{
    class Brana
    {
        private Rectangle gate;
        public static Rectangle brana1 = new Rectangle();
        public Brana(Grid MRIZ, int vlevo, int nahoru)
        {
            gate = new Rectangle();
            gate.Width = 50;
            gate.Height = 50;
            gate.Margin = new Thickness(vlevo, nahoru, 0, 0);
            gate.VerticalAlignment = VerticalAlignment.Top;
            gate.HorizontalAlignment = HorizontalAlignment.Left;
            MRIZ.Children.Add(gate);
            ImageBrush Branatextura = new ImageBrush();
            Branatextura.ImageSource = new BitmapImage(new Uri("../../Resources/brana.png", UriKind.Relative));
            gate.Fill = Branatextura;
            Panel.SetZIndex(gate, 1);
            brana1 = gate;
        }
        public void otevrise()
        {
            ImageBrush Branaotevrena = new ImageBrush();
            Branaotevrena.ImageSource = new BitmapImage(new Uri("../../Resources/otevrenabrana.png", UriKind.Relative));
            gate.Fill = Branaotevrena;
        }
    }
}
