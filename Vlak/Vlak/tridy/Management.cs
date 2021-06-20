using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlak
{
    static class Management
    {
        private static int skore = 0;
        public static int maxskore = 0;
        public static bool createditor = false;
        private static bool pohyb = true;
        private static string vyber;
        public static int Skore
        {
            get
            {
                return skore;
            }
            set
            {
                skore = value;
            }
        }
        public static bool Pohyb            //pohyb vlaku 
        {
            get
            {
                return pohyb;
            }
            set
            {
                pohyb = value;
            }
        }
        public static bool Sebrano         
        {
            get
            {
                return skore == maxskore;
            }
        }    
        public static string Vyber
        {
            get
            {
                return vyber;
            }
            set
            {
                vyber = value;
            }

        } 
    }
}
