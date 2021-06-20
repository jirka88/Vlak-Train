using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlak
{
    class Mapa
    {
        private Vagon[,] map = new Vagon[18, 22];
        public Mapa()
        {

        }
        public void nastav(int radek, int sloupec, Vagon hodnota)
        {
            map[radek, sloupec] = hodnota;
        }
        public Vagon precti(int radek, int sloupec)
        {
            return map[radek, sloupec];                
        }
    }
}
