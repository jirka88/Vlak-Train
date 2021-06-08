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
        private ImageBrush Branatextura = new ImageBrush();     
        public static Rectangle brana1 { get; private set; } = new Rectangle();           
        public Brana(Grid MRIZ, int vlevo, int nahoru)
        {
            gate = new Rectangle()
            {
                Width = 50,
                Height = 50,
                Margin = new Thickness(vlevo, nahoru, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };     
            MRIZ.Children.Add(gate);    
            Branatextura.ImageSource = new BitmapImage(new Uri("../../Resources/brana.png", UriKind.Relative));
            gate.Fill = Branatextura;
            Panel.SetZIndex(gate, 1);
            brana1 = gate;
        }
        public void otevrise()
        {  
            Branatextura.ImageSource = new BitmapImage(new Uri("../../Resources/otevrenabrana.png", UriKind.Relative));
            gate.Fill = Branatextura;
        }

        
    }
}
