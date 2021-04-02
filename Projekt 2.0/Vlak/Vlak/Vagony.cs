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
    class Vagony
    {
        protected Rectangle vagonn;
        protected SoundPlayer sebers = new SoundPlayer(Properties.Resources.seber);
        protected ScaleTransform flipvlak = new ScaleTransform();
        protected RotateTransform Transformace = new RotateTransform();
        public static Grid MRIZ;
        public static List<Rectangle> vagonysvlakem = new List<Rectangle>();
        protected Vagony(int vlevo, int nahoru)
        {
            vagonn = new Rectangle();
            vagonn.Width = 50;
            vagonn.Height = 50;
            vagonn.Margin = new Thickness(vlevo, nahoru, 0, 0);
            vagonn.VerticalAlignment = VerticalAlignment.Top;
            vagonn.HorizontalAlignment = HorizontalAlignment.Left;
            MRIZ.Children.Add(vagonn);
        }
    }
    class Vagon_1 : Vagony
    {
        public Vagon_1(int vlevo, int nahoru) : base(vlevo, nahoru)
        {
            ImageBrush diamond = new ImageBrush();
            diamond.ImageSource = new BitmapImage(new Uri(@"../../Resources/Diamond.png", UriKind.Relative));
            vagonn.Fill = diamond;
        }
        public void seber(int x, int y, int cRadku, int cSloupce)
        {
            vagonysvlakem.Add(vagonn);
            sebers.Play();
            ImageBrush vagon = new ImageBrush();
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon1.png", UriKind.Relative));
            vagonn.Fill = vagon;
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
        }
    }
    class vagon_2 : Vagony
    {
        public vagon_2(int vlevo, int nahoru) : base(vlevo, nahoru)
        {
            ImageBrush diamond = new ImageBrush();
            diamond.ImageSource = new BitmapImage(new Uri(@"../../Resources/koruna.png", UriKind.Relative));
            vagonn.Fill = diamond;
        }
        public void seber(int x, int y, int cRadku, int cSloupce)
        {
            vagonysvlakem.Add(vagonn);
            sebers.Play();
            ImageBrush vagon = new ImageBrush();
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon2.png", UriKind.Relative));
            vagonn.Fill = vagon;
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
        }

    }
    class vagon_3 : Vagony
    {
        public vagon_3(int vlevo, int nahoru) : base(vlevo, nahoru)
        {
            ImageBrush diamond = new ImageBrush();
            diamond.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom2.png", UriKind.Relative));
            vagonn.Fill = diamond;
        }
        public void seber(int x, int y, int cRadku, int cSloupce)
        {
            vagonysvlakem.Add(vagonn);
            sebers.Play();
            ImageBrush vagon = new ImageBrush();
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagony/vagon3.png", UriKind.Relative));
            vagonn.Fill = vagon;
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
        }

    }

}
