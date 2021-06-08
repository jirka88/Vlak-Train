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
        private Rectangle polickoEditor;
        private ImageBrush tex = new ImageBrush();
        private string ID = string.Empty;
        private bool nalez = false;       
      
        public static List<string> PolickaList { get; private set; } = new List<string>();
        public static List<Rectangle> CtvereckyList { get; private set; } = new List<Rectangle>();
        public Policko(int sloupec, int radek, string item)
        {
            polickoEditor = new Rectangle()
            {
                Width = 50,
                Height = 50,
                Fill = tex,
                Margin = new Thickness(sloupec, radek, 0, 0),
                Stroke = new SolidColorBrush(Colors.Red),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            polickoEditor.MouseLeftButtonDown += Pridani;
            switch (item)
            {
                case "1":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/Diamond.png", UriKind.Relative));                 
                    ID = "1";
                    break;
                case "2":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/koruna.png", UriKind.Relative));                 
                    ID = "2";
                    break;
                case "3":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom2.png", UriKind.Relative));                   
                    ID = "3";
                    break;
                case "j":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/jablko.png", UriKind.Relative));                 
                    ID = "j";
                    break;
                case "e":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/emerald.png", UriKind.Relative));             
                    ID = "e";
                    break;
                case "r":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/emerald.png", UriKind.Relative));                 
                    ID = "r";
                    break;
                case "f":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/ryba.png", UriKind.Relative));                
                    ID = "f";
                    break;
                case "t":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/tresen.png", UriKind.Relative));                 
                    ID = "t";
                    break;
                case "p":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/pc.png", UriKind.Relative));
                    ID = "p";
                    break;
                case "d":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/dort.png", UriKind.Relative));
                    ID = "d";
                    break;
                case "V":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/vlak.png", UriKind.Relative));               
                    ID = "V";
                    break;
                case "B":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/brana.png", UriKind.Relative));
                    if (radek == 0 || radek == 650 || sloupec == 0 || sloupec == 1000)
                    {
                        ID = "9B";
                    }
                    else
                    {
                        ID = "B";
                    }                                  
                    break;
                case "9":
                    tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));                 
                    if (radek == 0 || radek == 650 || sloupec == 0 || sloupec == 1000)
                    {                                             
                        ID = "9P";
                    }
                    else
                    {                                             
                        ID = "9";
                    }               
                    break;
                case "0":
                    polickoEditor.Fill = new SolidColorBrush(Colors.Black);
                    ID = "0";
                    break;
                case "z":
                    {
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/zmrzlina.png", UriKind.Relative));
                        ID = "z";
                    }
                    break;
                case "a":
                    {
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/auto.png", UriKind.Relative));
                        ID = "a";
                    }
                    break;
            } 
            MRIZ2.Children.Add(polickoEditor);    
            PolickaList.Add(ID);
            CtvereckyList.Add(polickoEditor); 
        }              
        private void Pridani(object sender, MouseButtonEventArgs e)
        {
            int index = CtvereckyList.IndexOf((Rectangle)(sender)); 
            if (ID == "9P" || ID == "9B" && Management.Vyber != "0")
            {
                if(Management.Vyber == "B")
                {
                    foreach (string policko in PolickaList)
                    {
                        if (policko == "B" || policko == "9B")
                        {
                            nalez = true;
                            break;
                        }
                    }
                    if (nalez)
                    {
                        MessageBox.Show("Nelze sem přidat!");
                    }
                    else
                    {
                        if(index == 0 || index == 20 || index == 273|| index == 293)
                        {
                            MessageBox.Show("Nelze přidat do rohu!");
                        }
                        else
                        {
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/brana.png", UriKind.Relative));
                            polickoEditor.Fill = tex;
                            ID = "9B";
                            PolickaList[index] = ID;
                        }                     
                    }
                }
                else
                {
                    if(Management.Vyber == "0")
                    {
                        MessageBox.Show("nelze odebrat");
                    }
                    else
                    {
                        MessageBox.Show("nelze sem přidat");
                    }                    
                }
            }          
            else
            {
                switch(Management.Vyber)
                {        
                    case "diamond":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/diamond.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "1";
                        break;
                    case "koruna":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/koruna.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "2";
                        break;
                    case "V":                       
                        foreach (string policko in PolickaList)
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
                            polickoEditor.Fill = tex;
                            ID = "V";
                        }                         
                        break;
                    case "0":
                        if(ID != "9B")
                        {
                            polickoEditor.Fill = new SolidColorBrush(Colors.Black);
                            ID = "0";
                        }                    
                        else
                        {                        
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                            polickoEditor.Fill = tex;
                            ID = "9P";
                        }                    
                        break;
                    case "B":
                        foreach (string policko in PolickaList)
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
                            polickoEditor.Fill = tex;
                            ID = "B";                        
                        }
                        break;
                    case "9":
                        if (PolickaList[index + 1] == "9B" || PolickaList[index - 1] == "9B" || PolickaList[index + 21] == "9B" || PolickaList[index - 21] == "9B") 
                        {
                            MessageBox.Show("Nelze dohrát!"); //pokud chceme před bránou postavit zeď == nelze dohrát 
                        }
                        else
                        {
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                            polickoEditor.Fill = tex;
                            ID = "9";
                        }
                        break;
                    case "e":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/emerald.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "e";
                        break;
                    case "3":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/strom2.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "3";
                        break;
                    case "j":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/jablko.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "j";
                        break;
                    case "r":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/redstone.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "r";
                        break;
                    case "f":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/ryba.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "f";
                        break;
                    case "t":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/tresen.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "t";
                        break;
                    case "z":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/zmrzlina.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "z";
                        break;
                    case "d":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/dort.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "d";
                        break;
                    case "a":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/auto.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "a";
                        break;
                    case "p":
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/pc.png", UriKind.Relative));
                        polickoEditor.Fill = tex;
                        ID = "p";
                        break;
                }                
                PolickaList[index] = ID;
            }
        }

        public static void Reset()
        {
            PolickaList.Clear();
            CtvereckyList.Clear();
        }
    }
}
