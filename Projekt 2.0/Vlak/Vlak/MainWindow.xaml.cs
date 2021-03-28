﻿using System;
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
                Rectangle posledni = vagony.vagonysvlakem.First();
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
                vagony.vagonysvlakem.RemoveAt(0);
                vagony.vagonysvlakem.Add(posledni);
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

            foreach (var item in vagony.vagonysvlakem)         //kontrola samotné kolize vlaku s vagonem 
            {
                if (vlakLeft == item.Margin.Left && vlakTop == item.Margin.Top)
                {
                    smrt(down);
                    break;
                }
            }
           
            foreach (var item in hranice.wall)
            {
                if(vlakLeft == item.Margin.Left && vlakTop == item.Margin.Top)
                {
                    smrt(down);
                    break;
                }
            }
            if (pohyb)
            {
                if (vlakLeft < 1000 && vlakLeft > 0 && vlakTop > 0 && vlakTop < 650)     //pohyb 
                {
                    vlakV.Margin = new Thickness(vlakLeft, vlakTop, 0, 0);
                }

                else if (vlakLeft == Brana.brana1.Margin.Left && vlakTop == Brana.brana1.Margin.Top) //vstup do brány
                {
                    if (MainWindow.skore == MainWindow.maxskore)
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
    class vagony
    {
        protected Rectangle vagonn;
        protected SoundPlayer sebers = new SoundPlayer(Properties.Resources.seber);
        protected ScaleTransform flipvlak = new ScaleTransform();
        protected RotateTransform Transformace = new RotateTransform();
        public static Grid MRIZ;
        public static List<Rectangle> vagonysvlakem = new List<Rectangle>();
        protected vagony(int vlevo, int nahoru)
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
    class vagon_1 : vagony
    {        
        public vagon_1(int vlevo, int nahoru) : base(vlevo, nahoru)
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
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagon1.png", UriKind.Relative));
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
    class vagon_2 : vagony
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
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagon2.png", UriKind.Relative));
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
    class vagon_3 : vagony
    {
        public vagon_3(int vlevo, int nahoru) : base(vlevo, nahoru)
        {
            ImageBrush diamond = new ImageBrush();
            diamond.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom.png", UriKind.Relative));
            vagonn.Fill = diamond;
        }
        public void seber(int x, int y, int cRadku, int cSloupce)
        {
            vagonysvlakem.Add(vagonn);
            sebers.Play();
            ImageBrush vagon = new ImageBrush();
            vagon.ImageSource = new BitmapImage(new Uri(@"../../Resources/vagon3.png", UriKind.Relative));
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
  
    public partial class MainWindow : Window
    {
        mapa rozlozeni;
        MessageBoxResult odpoved;
        int krok = 0;
        public static int skore = 0;
        public static int maxskore = 0;
        int level = 1;
        bool mamvagon = false;
        bool vyhra = true;
        bool heslo = false;
        bool start = false;
        

        vlak vlakname;
        hranice Wall;
        Brana Gate;
        vagon_1 Diamond;
        vagon_2 koruna;
        vagon_3 strom;
        StreamReader cti;  
        DispatcherTimer casvlak = new DispatcherTimer();    
       

        int x = 0;
        int y = 0;
        public MainWindow()
        {
            InitializeComponent();
            mapa rozlozeni = new mapa();
            vlak.MRIZ = pole;
            vagony.MRIZ = pole;
            hranice.MRIZ = pole;
            casvlak.Interval = new TimeSpan(0, 0, 0, 0, 250);
            casvlak.Tick += pohybA;            
            
        }


        private void pohyb(object sender, KeyEventArgs e)
        {
            if (start)
            {
                if (vlak.pohyb && vyhra && !heslo)
                {
                    switch (e.Key)
                    {
                        case Key.Right:
                            x = 50;
                            y = 0;
                            casvlak.Start();
                            break;
                        case Key.Up:
                            x = 0;
                            y = -50;
                            casvlak.Start();
                            break;
                        case Key.Down:
                            x = 0;
                            y = 50;
                            casvlak.Start();
                            break;
                        case Key.Left:
                            x = -50;
                            y = 0;
                            casvlak.Start();
                            break;
                        case Key.F4:
                            password.Visibility = Visibility.Visible;
                            heslo = true;
                            casvlak.Stop();
                            break;
                    }
                }
                else if (!vyhra)
                {
                    switch (e.Key)
                    {
                        case Key.Enter:
                            level += 1;
                            zacatek();
                            casvlak.Stop();
                            break;
                    }
                }
                else if (heslo)
                {
                    zkontrolujheslo(e);
                }
            }
        }
        private void pohybA(object sender, EventArgs e)
        {
            if (vlak.pohyb)
            {
                krok++;
                Thickness pozice = vlakname.pohybovani(x, y);        //aktualní pozice vlaku se uloží do thickness
                int cRadku = Convert.ToInt32(pozice.Top) / 50;
                int cSloupce = Convert.ToInt32(pozice.Left) / 50;
                kroky.Content = "Kroky: " + krok;
                vagony vagony = rozlozeni.precti(cRadku, cSloupce);

                if (vagony != null)                 
                {
                    if (vagony.GetType().Name == "vagon_1")
                    {
                        ((vagon_1)vagony).seber(x, y, cRadku, cSloupce);
                    }
                    else if (vagony.GetType().Name == "vagon_2")
                    {
                        ((vagon_2)vagony).seber(x, y, cRadku, cSloupce);
                    }
                    else if (vagony.GetType().Name == "vagon_3")
                    {
                        ((vagon_2)vagony).seber(x, y, cRadku, cSloupce);
                    }

                    rozlozeni.nastav(cRadku, cSloupce, null);
                    mamvagon = true;

                    skore += 10;
                    Skore.Content = "Skore: " + skore;

                    if (skore == maxskore)                                //okamžité otevření brány, když máme sebrané všechny vagony
                    {
                        Gate.otevrise();
                    }
                }
                else
                {
                    if (mamvagon)
                    {
                        vlakname.pripoj(cRadku, cSloupce, x, y);
                        if (pozice.Top == Brana.brana1.Margin.Top && pozice.Left == Brana.brana1.Margin.Left)            //výhra pokud je dodržena podmínka
                        {                  
                            casvlak.Stop();
                            vyhra = false;
                        }
                    }
                }
            }
            else
            {
                casvlak.Stop();
                odpoved = MessageBox.Show("Prohrál jsi! Přeješ znovu začít hru ?", "1.scena", MessageBoxButton.YesNo);
                switch (odpoved)
                {
                    case MessageBoxResult.Yes:
                        zacatek();
                        break;

                    case MessageBoxResult.No:
                        Application.Current.Shutdown();
                        break;
                }
            }
        }
        private void zkontrolujheslo(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (hesla.Text.ToLower())
                {
                    case "diamond":
                        level = 1;
                        break;
                    case "král":
                        level = 2;
                        break;
                    default:
                        break;
                    case "mydlo":
                        level = 3;
                        break;
                }
                zacatek();
                heslo = false;
                hesla.Text = "Heslo";
                password.Visibility = Visibility.Hidden;
            }
        }
        private void zacatek()
        {
          
            int left = 0;
            int vlevo = 0;
            int nahoru = -50;
          
            MainWindow.maxskore = 0;
            MainWindow.maxskore += MainWindow.skore;
            vagon_1.vagonysvlakem.Clear();
            hranice.wall.Clear();
            vlak.pohyb = true;
            mamvagon = false;
            vyhra = true;

            pole.Children.Clear();
            pole.Children.Add(Skore);
            pole.Children.Add(kroky);
            pole.Children.Add(scena);
            pole.Children.Add(jmenolevelu);
           

            
            Skore.Content = "Skore: " + MainWindow.skore;                   //reset skore

            krok = 0;
            kroky.Content = "Kroky: " + krok;  
           
                                                   
            rozlozeni = new mapa();                                         //vytvoření nové mapy
            
            scena.Content = $"Scena " + level;
        
            int down = 650;
            for (int i = 0; i < 3; i++)
            {
                down += 50;
                Wall = new hranice(0, down);
                Wall = new hranice(1000, down);
            }
            for (int a = 0; a < 19; a++)
            {
                left += 50;                                
                Wall = new hranice(left, 800);             
            }

            //LEVELOVÁNÍ

            cti = new StreamReader(@"../../sceny/level" + level + ".txt");
            string name = cti.ReadLine();
            jmenolevelu.Content = name;
            for (int i = 0; i < 14; i++)
            {            
                nahoru += 50;
                vlevo = 0;
                string[] objekty = cti.ReadLine().Split(' ');                              
               
                for (int j = 0; j < 21; j++)
                {
                    switch (objekty[j])
                    {
                        case "1":
                            Diamond = new vagon_1(vlevo, nahoru);
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, Diamond);
                            MainWindow.maxskore += 10;
                            break;
                        case "2":
                            koruna = new vagon_2(vlevo, nahoru);
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, koruna);
                             MainWindow.maxskore += 10;
                            break;
                        case "3":
                            strom = new vagon_3(vlevo, nahoru);
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, strom);
                            MainWindow.maxskore += 10;
                            break;
                        case "9":                          
                               Wall = new hranice(vlevo, nahoru);                                                        
                            break;
                        case "10":                           
                                Gate = new Brana(pole, vlevo, nahoru);                                                       
                            break;
                        case "11":                            
                                vlakname = new vlak(vlevo, nahoru);
                            break;
                    }                                                              
                    vlevo += 50;              
                }                                 
            }          
            cti.Close();           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start = true;
            pole.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Hidden;
            zacatek();       
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

      

        private void passworddelete(object sender, RoutedEventArgs e)
        {
                           
                hesla.Text = "";          
        }
    }
}
       
         
    
    


