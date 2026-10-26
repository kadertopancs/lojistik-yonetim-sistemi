
namespace LojistikUygulamasi.Desktop
{
    public class Arac
    {
        public int AracId { get; set; }

        public string Plaka { get; set; } = string.Empty;

        public string Marka { get; set; } = string.Empty;

        public decimal KapasiteTon { get; set; }

        public bool AktifMi { get; set; } = true;

        public int ModelYili { get; set; }

        public bool BilgilerGecerliMi()
        {
            return KapasiteTon > 0 && ModelYili >= 2000;
        }
    }
}
