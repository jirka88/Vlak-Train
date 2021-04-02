using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlak
{
    class Mapa
    {
        private Vagony[,] map = new Vagony[18, 22];
        public Mapa()
        {

        }
        public void nastav(int radek, int sloupec, Vagony hodnota)
        {
            map[radek, sloupec] = hodnota;

        }
        public Vagony precti(int radek, int sloupec)
        {
            return map[radek, sloupec];                 //zjistí jestli jsme na vagonu
        }
    }
}
