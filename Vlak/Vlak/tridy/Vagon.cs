using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using System.IO;

namespace Vlak
{
    class Vagon
    {
        private Rectangle vagonn;
        private SoundPlayer sebers = new SoundPlayer(Properties.Resources.seber);
        private ScaleTransform flipvlak = new ScaleTransform();
        private RotateTransform Transformace = new RotateTransform();
        public static Grid MRIZ;
        public static List<Rectangle> vagonysvlakem { get; private set; } = new List<Rectangle>();
        public static List<string> hazeni { get; private set; } = new List<string>();
        private ImageBrush textura = new ImageBrush();
        private ImageBrush vagon = new ImageBrush();
        private ImageBrush vagon1 = new ImageBrush();
        private ImageBrush vagon2 = new ImageBrush();
        private ImageBrush vagon3 = new ImageBrush();
        private ImageBrush vagon4 = new ImageBrush();
        private ImageBrush vagon5 = new ImageBrush();
        private ImageBrush vagon6 = new ImageBrush();
        private ImageBrush vagon7 = new ImageBrush();
        private ImageBrush vagon8 = new ImageBrush();
        private string identifikace = "";

        public Vagon(int vlevo, int nahoru, string typvagonu)
        {
            vagonn = new Rectangle();
            vagonn.Width = 50;
            vagonn.Height = 50;
            vagonn.Margin = new Thickness(vlevo, nahoru, 0, 0);
            vagonn.VerticalAlignment = VerticalAlignment.Top;
            vagonn.HorizontalAlignment = HorizontalAlignment.Left;

            MRIZ.Children.Add(vagonn);

            switch (typvagonu)
            {
                case "diamond":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/Diamond.png", UriKind.Relative));
                    identifikace = "diamond";
                    break;
                case "koruna":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/koruna.png", UriKind.Relative));
                    identifikace = "koruna";
                    break;
                case "strom":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom2.png", UriKind.Relative));
                    identifikace = "strom";
                    break;
                case "jablko":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/jablko.png", UriKind.Relative));
                    identifikace = "jablko";
                    break;
                case "emerald":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/emerald.png", UriKind.Relative));
                    identifikace = "emerald";
                    break;
                case "redstone":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/redstone.png", UriKind.Relative));
                    identifikace = "redstone";
                    break;
                case "ryba":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/ryba.png", UriKind.Relative));
                    identifikace = "ryba";
                    break;
                case "tresen":
                    textura.ImageSource = new BitmapImage(new Uri(@"../../Resources/tresen.png", UriKind.Relative));
                    identifikace = "tresen";
                    break;
            }
            vagonn.Fill = textura;
        }

        public void seber(int x, int y, int cRadku, int cSloupce)
        {
            hazeni.Add(identifikace);       
            vagonysvlakem.Add(vagonn);
            sebers.Play();  
            vagonn.RenderTransformOrigin = new Point(0.5, 0.5);
       
            cSloupce *= 50;
            cRadku *= 50;
            switch (x)
            {
                case 50:
                    vagonn.Margin = new Thickness(cSloupce - 50, cRadku, 0, 0);
                    flipvlak.ScaleX = 1;
                    vagonn.RenderTransform = flipvlak;
                    break;

                case -50:
                    vagonn.Margin = new Thickness(cSloupce + 50, cRadku, 0, 0);
                    flipvlak.ScaleX = -1;
                    vagonn.RenderTransform = flipvlak;
                    break;
            }
            switch (y)
            {
                case 50:
                    vagonn.Margin = new Thickness(cSloupce, cRadku - 50, 0, 0);
                    Transformace = new RotateTransform(90);
                    vagonn.RenderTransform = Transformace;
                    break;

                case -50:
                    vagonn.Margin = new Thickness(cSloupce, cRadku + 50, 0, 0);
                    Transformace = new RotateTransform(270);
                    vagonn.RenderTransform = Transformace;
                    break;
            }
            prehod();
        }
        public void prehod()
        {       
            int i = Vagon.vagonysvlakem.Count() - 1;                                                          
            foreach (Rectangle id in vagonysvlakem)
            {      
                switch (hazeni[i])
                {
                    case "diamond":
                        vagon1.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon1.png", UriKind.Relative));
                        id.Fill = vagon1;              
                        break;
                    case "koruna":                       
                        vagon2.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon2.png", UriKind.Relative));
                        id.Fill = vagon2;                                     
                        break;
                    case "strom":                   
                        vagon3.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon3.png", UriKind.Relative));
                        id.Fill = vagon3;                   
                        break;
                    case "jablko":                   
                        vagon4.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon4.png", UriKind.Relative));
                        id.Fill = vagon4;
                        break;
                    case "emerald":
                        vagon5.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon5.png", UriKind.Relative));
                        id.Fill = vagon5;
                        break;
                    case "redstone":
                        vagon6.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon6.png", UriKind.Relative));
                        id.Fill = vagon6;
                        break;
                    case "ryba":
                        vagon7.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon7.png", UriKind.Relative));
                        id.Fill = vagon7;
                        break;
                    case "tresen":
                        vagon8.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon8.png", UriKind.Relative));
                        id.Fill = vagon8;
                        break;
                }
                i--;
                     
            }
        
        }

    }

}
