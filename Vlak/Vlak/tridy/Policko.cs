using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vlak
{
    class Policko
    {
        public static Grid MRIZ2;
        private Rectangle polickoU;
        private ImageBrush tex = new ImageBrush();
        private string ID = string.Empty;
        private bool nalez = false;
        public static string vyber = "";         
        public static List<string> policka { get; private set; } = new List<string>();
        public static List<Rectangle> ctverecky { get; private set; } = new List<Rectangle>();
        public Policko(int sloupec, int radek)
        {
            polickoU = new Rectangle();
            polickoU.Width = 50;
            polickoU.Height = 50;
            if (radek == 0 || radek == 650 || sloupec == 0 || sloupec == 1000)
            {
                tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                polickoU.Fill = tex;
                ID = "9P";              
            }               
            else
            {
                polickoU.Fill = new SolidColorBrush(Colors.Black);
                ID = "0";                
            }        
            polickoU.Margin = new Thickness(sloupec, radek, 0, 0);
            polickoU.Stroke = new SolidColorBrush(Colors.Red);
            polickoU.VerticalAlignment = VerticalAlignment.Top;
            polickoU.HorizontalAlignment = HorizontalAlignment.Left;
            polickoU.MouseLeftButtonDown += Pridani;
            MRIZ2.Children.Add(polickoU);    
            policka.Add(ID);
            ctverecky.Add(polickoU);         
        }              

        private void Pridani(object sender, MouseButtonEventArgs e)
        {
            int index = ctverecky.IndexOf((Rectangle)(sender)); 
            if (ID == "9P" || ID == "9B" && vyber != "0")
            {
                if(vyber == "B")
                {
                    foreach (string policko in policka)
                    {
                        if (policko == "B" || policko == "9B")
                        {
                            nalez = true;
                            break;
                        }
                    }
                    if (nalez)
                    {
                        MessageBox.Show("Nelze přidat");
                    }
                    else
                    {
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/brana.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "9B";
                        policka[index] = ID;
                    }
                }
                else
                {
                    MessageBox.Show("nelze sem přidat");
                }
            }          
            else
            {
                switch(vyber)
                {        
                    case "diamond":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/diamond.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "1";
                        break;
                    case "koruna":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/koruna.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "2";
                        break;
                    case "V":                       
                        foreach (string policko in policka)
                        {
                            if (policko == "V")
                            {
                                nalez = true;
                                break;
                            }
                        }
                        if (nalez)
                        {
                            MessageBox.Show("Nelze přidat");
                        }
                        else
                        {
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/vlak.png", UriKind.Relative));
                            polickoU.Fill = tex;
                            ID = "V";
                        }                         
                        break;
                    case "0":
                        if(ID != "9B")
                        {
                            polickoU.Fill = new SolidColorBrush(Colors.Black);
                            ID = "0";
                        }
                        else
                        {                        
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                            polickoU.Fill = tex;
                            ID = "9P";
                        }                    
                        break;
                    case "B":
                        foreach (string policko in policka)
                        {
                            if (policko == "B" || policko == "9B")
                            {
                                nalez = true;
                                break;
                            }
                        }
                        if (nalez)
                        {
                            MessageBox.Show("Nelze přidat");
                        }
                        else
                        {
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/brana.png", UriKind.Relative));
                            polickoU.Fill = tex;
                            ID = "B";                        
                        }
                        break;
                    case "9":                     
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "9";                                               
                        break;
                    case "e":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/emerald.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "e";
                        break;
                    case "3":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom2.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "3";
                        break;
                    case "j":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/jablko.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "j";
                        break;
                    case "r":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/redstone.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "r";
                        break;
                    case "f":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/ryba.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "f";
                        break;
                    case "t":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/tresen.png", UriKind.Relative));
                        polickoU.Fill = tex;
                        ID = "t";
                        break;
                }                
                policka[index] = ID;
            }
        }

      
    }
}
