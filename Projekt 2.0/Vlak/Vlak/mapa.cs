using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlak
{
    class mapa
    {
        private vagony[,] map = new vagony[18, 22];
        public mapa()
        {

        }
        public void nastav(int radek, int sloupec, vagony hodnota)
        {
            map[radek, sloupec] = hodnota;

        }
        public vagony precti(int radek, int sloupec)
        {
            return map[radek, sloupec];                 //zjistí jestli jsme na vagonu
        }
    }
}
