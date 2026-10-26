using System;
using System.Collections.Generic;
using System.Text;

namespace LojistikUygulamasi
{
    internal class Arac
    {
        public int AracId { get; set; }
        public string Plaka { get; set; } = string.Empty;
        public string Marka { get; set; } = string.Empty;
        public decimal KapasiteTon { get; set; }
        public bool AktifMi { get; set; } = true; 
        public int ModelYili { get; set; }
        public bool BilgilerGecerliMi()
        {
            if (KapasiteTon <= 0)
            {
                return false;
            }

            if (ModelYili < 2000)
            {
                return false;
            }

            return true;
        }


    }
}
