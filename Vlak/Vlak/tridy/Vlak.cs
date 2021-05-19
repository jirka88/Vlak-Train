using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vlak
{
    class vlak
    {
        private Rectangle vlakV;
        public static Grid MRIZ;
        private ScaleTransform flipvlak = new ScaleTransform();
        private RotateTransform Transformace = new RotateTransform();

        private ImageBrush znicenyVlak = new ImageBrush();
        private SoundPlayer Boom = new SoundPlayer(Properties.Resources.boom1);
        public static bool pohyb = true;
        public vlak(int vlevo, int nahoru)
        {
            vlakV = new Rectangle();
            vlakV.Width = 50;
            vlakV.Height = 50;
            vlakV.Margin = new Thickness(vlevo, nahoru, 0, 0);
            vlakV.VerticalAlignment = VerticalAlignment.Top;
            vlakV.HorizontalAlignment = HorizontalAlignment.Left;
            MRIZ.Children.Add(vlakV);
            ImageBrush vlakte = new ImageBrush();
            vlakte.ImageSource = new BitmapImage(new Uri(@"../../Resources/vlak.png", UriKind.Relative));
            znicenyVlak.ImageSource = new BitmapImage(new Uri(@"../../Resources/znicenyvlak.png", UriKind.Relative));
            vlakV.Fill = vlakte;
            Panel.SetZIndex(vlakV, 2);
        }
        public void pripoj(int Cradku, int cSloupce, int x, int y)
        {
            if (vlak.pohyb)
            {
                ScaleTransform flipvagon = new ScaleTransform();
                RotateTransform Transformace = new RotateTransform();
                Rectangle posledni = Vagon.vagonysvlakem.First();
                posledni.RenderTransformOrigin = new Point(0.5, 0.5);

                cSloupce *= 50;
                Cradku *= 50;
                switch (x)
                {
                    case 50:
                        posledni.Margin = new Thickness(cSloupce - 50, Cradku, 0, 0);
                        flipvagon.ScaleX = 1;
                        posledni.RenderTransform = flipvagon;
                        break;

                    case -50:
                        posledni.Margin = new Thickness(cSloupce + 50, Cradku, 0, 0);
                        flipvagon.ScaleX = -1;
                        posledni.RenderTransform = flipvagon;
                        break;
                }
                switch (y)
                {
                    case 50:
                        posledni.Margin = new Thickness(cSloupce, Cradku - 50, 0, 0);
                        Transformace = new RotateTransform(90);
                        posledni.RenderTransform = Transformace;
                        break;

                    case -50:
                        posledni.Margin = new Thickness(cSloupce, Cradku + 50, 0, 0);
                        Transformace = new RotateTransform(270);
                        posledni.RenderTransform = Transformace;
                        break;
                }
                Vagon.vagonysvlakem.RemoveAt(0);
                Vagon.vagonysvlakem.Add(posledni);

            }
        }
        public Thickness pohybovani(int right, int down)
        {
            vlakV.RenderTransformOrigin = new Point(0.5, 0.5);
            int vlakLeft = Convert.ToInt32(vlakV.Margin.Left + right);
            int vlakTop = Convert.ToInt32(vlakV.Margin.Top + down);
            switch (right) //pohyb textury vodorovně
            {
                case 50:
                    flipvlak.ScaleX = 1;
                    vlakV.RenderTransform = flipvlak;
                    break;

                case -50:
                    flipvlak.ScaleX = -1;
                    vlakV.RenderTransform = flipvlak;
                    break;
            }
            switch (down) //pohyb textury - nahoru dolu
            {
                case 50:
                    Transformace = new RotateTransform(90);
                    vlakV.RenderTransform = Transformace;
                    break;

                case -50:
                    Transformace = new RotateTransform(270);
                    vlakV.RenderTransform = Transformace;
                    break;
            }

            foreach (var item in Vagon.vagonysvlakem)         //kontrola samotné kolize vlaku s vagonem 
            {
                if (vlakLeft == item.Margin.Left && vlakTop == item.Margin.Top)
                {
                    smrt(down);
                    break;
                }
            }

            foreach (var item in Hranice.wall)
            {
                if (vlakLeft == item.Margin.Left && vlakTop == item.Margin.Top)
                {
                    smrt(down);
                    break;
                }
            }

            if (pohyb)
            {
                if (vlakLeft == Brana.brana1.Margin.Left && vlakTop == Brana.brana1.Margin.Top) //vstup do brány
                {
                    if (management.skore == management.maxskore)
                    {
                        vlakV.Margin = new Thickness(vlakLeft, vlakTop, 0, 0); //poslední krok vlaku do brány                                       
                        SoundPlayer playSound = new SoundPlayer(Properties.Resources.victory);
                        playSound.Play();
                    }
                    else
                    {
                        smrt(down);
                    }
                }
                else if (vlakLeft < 1000 && vlakLeft > 0 && vlakTop > 0 && vlakTop < 650)     //pohyb 
                {
                    vlakV.Margin = new Thickness(vlakLeft, vlakTop, 0, 0);
                }


            }
            return vlakV.Margin;
        }

        private void smrt(int down)
        {
            pohyb = false;
            switch (down)
            {
                case 50:
                case -50:
                    Transformace = new RotateTransform(0);
                    vlakV.RenderTransform = Transformace;
                    break;
            }
            Boom.Play();
            vlakV.Fill = znicenyVlak;
        }
    }

}
