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
        vlak train;
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
            Policko.MRIZ2 = editorG;
            casvlak.Interval = new TimeSpan(0, 0, 0, 0, 250);
            casvlak.Tick += TimerTick;
            video.MediaEnded += ZnovuSpustitVideo;
            video2.MediaEnded += ZnovuSpustitVideo2;
            video.Play();

        }
        private void ZnovuSpustitVideo2(object sender, RoutedEventArgs e)
        {
            video2.Position = new TimeSpan(0);
            video2.Play();
        }
        private void ZnovuSpustitVideo(object sender, RoutedEventArgs e)
        {
            video.Position = new TimeSpan(0);
            video.Play();
        }
        private void Pohyb(object sender, KeyEventArgs e)
        {
            if (start)
            {
                if (Management.Pohyb && vyhra && !heslo)
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
                            if(!Management.createditor)
                            {
                                password.Visibility = Visibility.Visible;
                                heslo = true;
                                casvlak.Stop();
                            }                                                     
                            break;
                    }
                }
                else if (!vyhra) //pokud jsme dojeli do brány 
                {
                    if(e.Key == Key.Enter)
                    {
                        if(!Management.createditor)
                        {                           
                            level += 1;
                            Zacatek();
                            casvlak.Stop();
                        }
                        else
                        {
                            Zacatek();
                            casvlak.Stop();
                        }
                        
                    }                                               
                }
                else if (heslo) //kontola hesla
                {
                    ZkontrolujHeslo(e);
                }
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (Management.Pohyb)
            {
                krok++;
                Thickness pozice = train.Pohybovani(x, y);        //aktualní pozice vlaku se uloží do thickness
                int cRadku = Convert.ToInt32(pozice.Top) / 50;
                int cSloupce = Convert.ToInt32(pozice.Left) / 50;
                kroky.Content = "Kroky: " + krok;
                Vagon vagony = rozlozeni.precti(cRadku, cSloupce);

                if (vagony != null)                 
                {
                    rozlozeni.precti(cRadku, cSloupce).Seber(x, y, cRadku, cSloupce);                                             
                    rozlozeni.nastav(cRadku, cSloupce, null);
                    mamvagon = true;

                    Management.Skore += 10;
                    Skore.Content = "Skore: " + Management.Skore;

                    if(Management.Sebrano)
                    {
                        Gate.otevrise();
                    }                 
                }
                else
                {
                    if (mamvagon)
                    {
                        train.Pripoj(cRadku, cSloupce, x, y);
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
                        Zacatek();
                        break;

                    case MessageBoxResult.No:
                        if(!Management.createditor)
                        {
                            skoreV2 = Management.Skore;
                        }                      
                        pole.Visibility = Visibility.Hidden;
                        menu.Visibility = Visibility.Visible;
                        start = false;
                        video.Play();
                        Kurzor();
                        break;
                }
            }
        }
        private void ZkontrolujHeslo(KeyEventArgs e)
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
                Zacatek();
                heslo = false;
                hesla.Text = "Heslo";
                password.Visibility = Visibility.Hidden;
            }
        }
        private void PasswordDelete(object sender, KeyboardFocusChangedEventArgs e)
        {
            hesla.Text = "";
        }
        private void Zacatek()
        {
            if (level != 15)
            {
                int vlevo = 0;
                int nahoru = -50;
                Management.maxskore = 0;
                Hranice.wall.Clear();
                Vagon.SmazVse();
                Management.Pohyb = true;
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
                if (!Management.createditor)
                {
                    if (Management.Skore == 0) //pokud přešel z editoru načte jeho skore zpátky
                    {
                        Management.maxskore += skoreV2;
                        Management.Skore = skoreV2;
                    }

                    else
                    {
                        Management.maxskore += Management.Skore;
                    }
                    cti = new StreamReader(@"../../sceny/level" + level + ".txt");
                    string name = cti.ReadLine();
                    jmenolevelu.Content = name;
                    pole.Children.Add(jmenolevelu);
                    pole.Children.Add(scena);
                }
                else
                {
                    Management.Skore = 0;
                    editorG.Visibility = Visibility.Hidden;
                    pole.Visibility = Visibility.Visible;
                    paleta.Visibility = Visibility.Hidden;
                    cti = new StreamReader(@"../../sceny/costumlevl.txt");
                }
                Skore.Content = "Skore: " + Management.Skore;
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
                                Management.maxskore += 10;
                                break;
                            case "2":
                                vagonek = new Vagon(vlevo, nahoru, "koruna");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "3":
                                vagonek = new Vagon(vlevo, nahoru, "strom");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "j":
                                vagonek = new Vagon(vlevo, nahoru, "jablko");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "e":
                                vagonek = new Vagon(vlevo, nahoru, "emerald");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "t":
                                vagonek = new Vagon(vlevo, nahoru, "tresen");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "r":
                                vagonek = new Vagon(vlevo, nahoru, "redstone");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "f":
                                vagonek = new Vagon(vlevo, nahoru, "ryba");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "9":
                                Wall = new Hranice(vlevo, nahoru);
                                break;
                            case "B":
                                Gate = new Brana(pole, vlevo, nahoru);
                                break;
                            case "V":
                                train = new vlak(vlevo, nahoru);
                                break;
                            case "z":
                                vagonek = new Vagon(vlevo, nahoru, "zmrzlina");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "d":
                                vagonek = new Vagon(vlevo, nahoru, "dort");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "a":
                                vagonek = new Vagon(vlevo, nahoru, "auto");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                            case "p":
                                vagonek = new Vagon(vlevo, nahoru, "pc");
                                rozlozeni.nastav(nahoru / 50, vlevo / 50, vagonek);
                                Management.maxskore += 10;
                                break;
                        }
                        vlevo += 50;
                    }
                    vlevo = 0;
                    int down = 650;
                    for (int k = 0; k < 3; k++)
                    {
                        down += 50;
                        Wall = new Hranice(0, down);
                        Wall = new Hranice(1000, down);
                    }
                    for (int a = 0; a < 19; a++)
                    {
                        vlevo += 50;
                        Wall = new Hranice(vlevo, 800);
                    }

                }
                cti.Close();
            }
            else
             {
                pole.Visibility = Visibility.Hidden;
                vyhraG.Visibility = Visibility.Visible;
             }
            }     
        //GUI
        private void StartHry(object sender, RoutedEventArgs e)
        {
            start = true;         
            Management.Skore = 0;
            Management.createditor = false;
            pole.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Hidden;
            video.Pause();
            Zacatek();       
        }
        private void Vypnuti(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }      
        private void OvladaniOkno(object sender, RoutedEventArgs e)
        {         
            video.Pause();
            video2.Play();
            menu.Visibility = Visibility.Hidden;
            ovladani.Visibility = Visibility.Visible;
        }
        private void ZpatkyNaMenu(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Visible;
            ovladani.Visibility = Visibility.Hidden;
            editorG.Visibility = Visibility.Hidden;
            paleta.Visibility = Visibility.Hidden;
            Management.createditor = false;
            video.Play();
            video2.Pause();
            Kurzor();
        }

        //editor
        private void Editor(object sender, RoutedEventArgs e)
        {       
            video.Pause();
            menu.Visibility = Visibility.Hidden;
            editorG.Visibility = Visibility.Visible;
            paleta.Visibility = Visibility.Visible;
            EditorLevlu();
        }     
        private void EditorLevlu()
        {
            Management.createditor = true;
            Policko.Reset();
            editorG.Children.Clear();               
            cti = new StreamReader(@"../../sceny/costumlevl.txt");
            int left = 0;
            int down = -50;
            for (int i = 0; i < 14; i++)
            {
                left = 0;
                down += 50;
                string[] objekty = cti.ReadLine().Split(' ');
                for (int a = 0; a < 21; a++)
                {
                    switch (objekty[a])
                    {
                        case "0":
                            polickoT = new Policko(left, down, "0");
                            break;
                        case "1":
                            polickoT = new Policko(left, down, "1");
                            break;
                        case "2":
                            polickoT = new Policko(left, down, "2");
                            break;
                        case "3":
                            polickoT = new Policko(left, down, "3");
                            break;
                        case "j":
                            polickoT = new Policko(left, down, "j");
                            break;
                        case "e":
                            polickoT = new Policko(left, down, "e");
                            break;
                        case "t":
                            polickoT = new Policko(left, down, "t");
                            break;
                        case "r":
                            polickoT = new Policko(left, down, "r");
                            break;
                        case "f":
                            polickoT = new Policko(left, down, "f");
                            break;
                        case "9":
                            polickoT = new Policko(left, down,"9");
                            break;
                        case "B":
                            polickoT = new Policko(left, down,"B");
                            break;
                        case "V":
                            polickoT = new Policko(left, down,"V");
                            break;
                        case "z":
                            polickoT = new Policko(left, down, "z");
                            break;
                        case "d":
                            polickoT = new Policko(left, down, "d");
                            break;
                        case "a":
                            polickoT = new Policko(left, down, "a");
                            break;
                        case "p":
                            polickoT = new Policko(left, down, "p");
                            break;
                    }                    
                    left += 50;
                }
            }
            cti.Close();
        }
            
        private void Editnuti(object sender, RoutedEventArgs e)
        {
            int uspesnystart = 0;
            int index = -1;
            bool detekce = false;       
            foreach (string objekt in Policko.PolickaList)
            {
                index++;
                if (objekt == "V" || objekt == "B" || objekt == "9B")
                {
                    uspesnystart++;
                    if (Policko.PolickaList[index + 1] == "9" && Policko.PolickaList[index - 1] == "9" && Policko.PolickaList[index + 21] == "9" && Policko.PolickaList[index - 21] == "9")
                    {
                        uspesnystart = 0;
                        break;                      
                    }
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
                foreach (string ctverec in Policko.PolickaList)
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
                Zacatek();
                Kurzor();
            }
            else
            {
                MessageBox.Show("Levl nelze dohrát");
            }
        }       
        private void Vyber(object sender, MouseButtonEventArgs e)
        {
            Management.Vyber = (string)((Image)sender).Tag;
            Cursor kurzor = null;
            switch ((string)((Image)sender).Tag)
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
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.vlak2));
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
                case "z":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.zmrzlina));
                    break;
                case "a":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.auto));
                    break;
                case "d":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.dort));
                    break;
                case "p":
                    kurzor = new Cursor(new MemoryStream(Properties.Resources.pc));
                    break;

            }            
            Cursor = kurzor;
        }

       //změna kurzoru na default 
        private void Kurzor()
        {
            Cursor = Cursors.Arrow;
        }

        private void Reset(object sender, RoutedEventArgs e)
        {  
            int radkovani = -1;
            vytvor = new StreamWriter(@"../../sceny/costumlevl.txt", false);
            for (int i = 0; i < Policko.CtvereckyList.Count(); i++)
            {                                        
                radkovani++;
                if (radkovani == 21)
                {
                    vytvor.WriteLine();
                    radkovani = 0;
                }
                if (Policko.PolickaList[i] != "9P")
                {
               
                    if (Policko.PolickaList[i] == "9B")
                    {                                                            
                            ImageBrush tex = new ImageBrush();
                            tex.ImageSource = new BitmapImage(new Uri(@"../../Resources/hranice.png", UriKind.Relative));
                            Policko.CtvereckyList[i].Fill = tex;
                            Policko.PolickaList[i] = "9P";
                            vytvor.Write("9" + " ");                                         
                    }
                    else
                    {
                       
                        Policko.CtvereckyList[i].Fill = new SolidColorBrush(Colors.Black);
                        Policko.PolickaList[i] = "0";
                        vytvor.Write("0" + " ");
                    }
                }
                else
                {
                    vytvor.Write("9" + " ");
                }
               
            }
            vytvor.Close();
        }
        private void CelkovaVyhra(object sender, RoutedEventArgs e)
        {
            level = 1;
            vyhraG.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Visible;
            video.Play();
        }
    }
}
       
         
    
    


