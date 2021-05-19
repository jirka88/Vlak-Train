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
    public partial class MainWindow : Window
    {
        Mapa rozlozeni;
        MessageBoxResult odpoved;
        int krok = 0;
        int radkovani = 0;
        int level = 1;
        int skoreV2 = 0;
        bool mamvagon = false;
        bool vyhra = true;
        bool heslo = false;
        bool start = false;    

        vlak vlakname;
        Hranice Wall;
        Brana Gate;
        Vagon vagonek;
        StreamReader cti;
        StreamReader ctihesla;
        DispatcherTimer casvlak = new DispatcherTimer();
        Policko polickoT;
        StreamWriter vytvor;
       
        int x = 0;
        int y = 0;
        public MainWindow()
        {
            InitializeComponent();                  
            Mapa rozlozeni = new Mapa(); 
            vlak.MRIZ = pole;
            Vagon.MRIZ = pole;
            Hranice.MRIZ = pole;
            Policko.MRIZ2 = edite;
            casvlak.Interval = new TimeSpan(0, 0, 0, 0, 250);
            casvlak.Tick += pohybA;
            video.MediaEnded += znovapustitvideo;
            video.Play();

        }

        private void znovapustitvideo(object sender, RoutedEventArgs e)
        {
            video.Position = new TimeSpan(0);
            video.Play();
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
                    if(e.Key == Key.Enter)
                    {
                        if(!management.createditor)
                        {                           
                            level += 1;
                            zacatek();
                            casvlak.Stop();
                        }
                        else
                        {
                            zacatek();
                            casvlak.Stop();
                        }
                        
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
                Vagon vagony = rozlozeni.precti(cRadku, cSloupce);

                if (vagony != null)                 
                {
                    rozlozeni.precti(cRadku, cSloupce).seber(x, y, cRadku, cSloupce);                                             
                    rozlozeni.nastav(cRadku, cSloupce, null);
                    mamvagon = true;

                    management.skore += 10;
                    Skore.Content = "Skore: " + management.skore;

                    if (management.skore == management.maxskore)                                //okamžité otevření brány, když máme sebrané všechny vagony
                    {
                        Gate.otevrise();
                    }
                }
                else
                {
                    if (mamvagon)
                    {
                        vlakname.pripoj(cRadku, cSloupce, x, y);
                        vagonek.prehod();
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
                odpoved = MessageBox.Show("Prohrál jsi! Přeješ znovu začít hru ? ", + level +".scena", MessageBoxButton.YesNo);
                switch (odpoved)
                {
                    case MessageBoxResult.Yes:
                        zacatek();
                        break;

                    case MessageBoxResult.No:
                        pole.Visibility = Visibility.Hidden;
                        menu.Visibility = Visibility.Visible;
                        start = false;
                        video.Play();
                        kurzor();
                        break;
                }
            }
        }
        private void zkontrolujheslo(KeyEventArgs e)
        {
          
            if (e.Key == Key.Enter)
            {
                radkovani = 0;
                ctihesla = new StreamReader(@"../../sceny/hesla.txt");
                while (!ctihesla.EndOfStream)
                {
                    radkovani++;
                    if (ctihesla.ReadLine() == hesla.Text.ToLower())
                    {
                        level = radkovani;
                        break;
                    }                
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
            
            management.maxskore = 0;

            Vagon.vagonysvlakem.Clear();
            Hranice.wall.Clear();
            Vagon.hazeni.Clear();

            vlak.pohyb = true;
            mamvagon = false;
            vyhra = true;

            pole.Children.Clear();         
            pole.Children.Add(kroky);
            pole.Children.Add(Skore);
       
            krok = 0;

            kroky.Content = "Kroky: " + krok;      
            scena.Content = $"Scena " + level;

            rozlozeni = new Mapa();         
            
            //LEVELOVÁNÍ
            if (!management.createditor)
            {               
                if (management.skore == 0) //pokud přešel z editoru načte jeho skore zpátky
                {
                    management.maxskore += skoreV2;
                    management.skore = skoreV2;
                }

                else
                {
                    management.maxskore += management.skore;
                }     
                
                cti = new StreamReader(@"../../sceny/level" + level + ".txt");
                string name = cti.ReadLine();
                jmenolevelu.Content = name;
                pole.Children.Add(jmenolevelu);
                pole.Children.Add(scena);
            }
            else
            {         
                management.skore = 0;
                edite.Visibility = Visibility.Hidden;
                pole.Visibility = Visibility.Visible;
                cti = new StreamReader(@"../../sceny/costumlevl.txt");             
            }

            Skore.Content = "Skore: " + management.skore;

            //načítaní z textového souboru 
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
                                vagonek = new Vagon(vlevo, nahoru, "diamond");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                management.maxskore += 10;
                                break;
                            case "2":
                                vagonek = new Vagon(vlevo, nahoru, "koruna");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                management.maxskore += 10;
                                break;
                            case "3":
                                vagonek = new Vagon(vlevo, nahoru, "strom");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                management.maxskore += 10;
                                break;
                            case "j":
                                vagonek = new Vagon(vlevo, nahoru, "jablko");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                management.maxskore += 10;
                                break;
                            case "e":
                            vagonek = new Vagon(vlevo, nahoru, "emerald");
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                            management.maxskore += 10;
                            break;
                        case "t":
                            vagonek = new Vagon(vlevo, nahoru, "tresen");
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                            management.maxskore += 10;
                            break;
                        case "r":
                            vagonek = new Vagon(vlevo, nahoru, "redstone");
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                            management.maxskore += 10;
                            break;
                        case "f":
                            vagonek = new Vagon(vlevo, nahoru, "ryba");
                            rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                            management.maxskore += 10;
                            break;
                        case "9":
                            Wall = new Hranice(vlevo, nahoru);
                            break;
                        case "B":
                            Gate = new Brana(pole, vlevo, nahoru);                        
                            break;
                        case "V":
                            vlakname = new vlak(vlevo, nahoru);
                            break;
                        }
                        vlevo += 50;
                    }

                int down = 650;
                for (int k = 0; k < 3; k++)
                {
                    down += 50;
                    Wall = new Hranice(0, down);
                    Wall = new Hranice(1000, down);
                }
                for (int a = 0; a < 19; a++)
                {
                    left += 50;
                    Wall = new Hranice(left, 800);
                }

            }
                cti.Close();                   
        }
        private void starthry(object sender, RoutedEventArgs e)
        {
            start = true;
            management.skore = 0;
            management.createditor = false;
            pole.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Hidden;
            video.Pause();
            zacatek();       
        }

        private void vypnuti(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
   
        private void passworddelete(object sender, KeyboardFocusChangedEventArgs e)
        {
            hesla.Text = "";
        }

        private void ovladaniG(object sender, RoutedEventArgs e)
        {
            skoreV2 = management.skore;
            video.Pause();
            menu.Visibility = Visibility.Hidden;
            ovladani.Visibility = Visibility.Visible;
        }

        private void zpatkynamenu(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Visible;
            ovladani.Visibility = Visibility.Hidden;
            edite.Visibility = Visibility.Hidden;
            management.createditor = false;
            video.Play();
            kurzor();
        }

        //editor

        private void editor(object sender, RoutedEventArgs e)
        {
            skoreV2 = management.skore;
            video.Pause();
            menu.Visibility = Visibility.Hidden;
            edite.Visibility = Visibility.Visible;     
            editorLevlu();
        }

        private void editorLevlu()
        {              
              management.createditor = true;
              Policko.policka.Clear();
              Policko.ctverecky.Clear();
              edite.Children.Clear();
              edite.Children.Add(Brana2);
              edite.Children.Add(btn6);
              edite.Children.Add(btn7);
              edite.Children.Add(diamond);
              edite.Children.Add(Koruna);
              edite.Children.Add(del);
              edite.Children.Add(vlakk);
              edite.Children.Add(bord);
              edite.Children.Add(emerald);
              edite.Children.Add(strom);
              edite.Children.Add(jablko);
              edite.Children.Add(redstone);
              edite.Children.Add(ryba);
              edite.Children.Add(rs);
             edite.Children.Add(tresen);
            int left = 0;
            int down = -50;
            for (int i = 0; i < 14; i++)
            {
                left = 0;
                down += 50;
                for (int a = 0; a < 21; a++)
                {                  
                    polickoT = new Policko(left, down);
                    left += 50;
                }
            }              
        }
            
        private void editnuti(object sender, RoutedEventArgs e)
        {
            int uspesnystart = 0;
            bool detekce = false;
            foreach (string objekt in Policko.policka)
            {
                if(objekt == "V" || objekt == "B" || objekt == "9B")
                {
                    uspesnystart++;
                  //  if(uspesnystart)
                }             
                else if(!detekce && objekt != "0" && objekt != "9P" && objekt != "9")
                {
                    uspesnystart++;
                    detekce = true;
                }
                else if(uspesnystart == 3)
                {
                    break;
                }
            } //kontrola => ve hře MUSÍ být vlak brána a alespoň 1 objekt na sebrání 

            if (uspesnystart == 3)
            {
                int pocet = -1;
                vytvor = new StreamWriter(@"../../sceny/costumlevl.txt", false);
                foreach (string ctverec in Policko.policka)
                {
                    pocet++;
                    if (pocet == 21)
                    {
                        vytvor.WriteLine();
                        pocet = 0;
                    }
                    switch (ctverec)
                    {
                        case "9P":
                            vytvor.Write("9" + " ");
                            break;
                        case "9B":
                            vytvor.Write("B" + " ");
                            break;
                        default:
                            vytvor.Write(ctverec + " ");
                            break;
                    }            
                }
                vytvor.Close();
                start = true;
                zacatek();
                kurzor();
            }
            else
            {
                MessageBox.Show("Levl nelze dohrát");
            }
        }       
        private void vyber(object sender, MouseButtonEventArgs e)
        {
            Policko.vyber = (((Image)sender).Tag).ToString();
            Cursor kurzor = null;
            switch (Policko.vyber)
            {
                case "e":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.emerald));
                    break;
                case "koruna":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.koruna));
                    break;
                case "diamond":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.diamond2));
                    break;
                case "3":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.strom));
                    break;
                case "j":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.jablko));
                    break;
                case "B":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.brana1));
                    break;
                case "V":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.vlak1));
                    break;
                case "9":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.hranice1));
                    break;
                case "0":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.krizek));
                    break;
                case "r":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.redstone));
                    break;
                case "f":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.ryba));
                    break;
                case "t":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.tresen));
                    break;

            }            
            Cursor = kurzor;
        }

       //změna kurzoru na default 
        private void kurzor()
        {
            Cursor = Cursors.Arrow;
        }

        private void reset(object sender, RoutedEventArgs e)
        {
            int i = -1;
            foreach (Rectangle item in Policko.ctverecky)
            {
                i++;
                if(Policko.policka[i] != "9P")
                {
                    if (Policko.policka[i] == "9B")
                    {
                        ImageBrush tex = new ImageBrush();
                        tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                        item.Fill = tex;
                        Policko.policka[i] = "9P";
                    }
                    else
                    {
                        item.Fill = new SolidColorBrush(Colors.Black);
                        Policko.policka[i] = "0";
                    }                  
                } 
               
            }
        }
    }
}
       
         
    
    


