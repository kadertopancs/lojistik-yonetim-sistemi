
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LojistikUygulamasi.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Arac> _araclar = new();

        public MainWindow()
        {
            InitializeComponent();
            AraclarDataGrid.ItemsSource = _araclar;
            KartlariGuncelle();
        }

        private void AracEkleButton_Click(object sender, RoutedEventArgs e)
        {
            string plaka = PlakaTextBox.Text.Trim().ToUpperInvariant();
            string marka = MarkaTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(plaka) ||
                string.IsNullOrWhiteSpace(marka))
            {
                MessageBox.Show("Lütfen plaka ve marka bilgilerini girin.");
                return;
            }

            if (!decimal.TryParse(KapasiteTextBox.Text, out decimal kapasite) ||
                kapasite <= 0)
            {
                MessageBox.Show("Pozitif bir kapasite girin. Örnek: 12,5");
                return;
            }

            if (_araclar.Any(a =>
                string.Equals(a.Plaka, plaka,
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Bu plaka zaten kayıtlı.");
                return;
            }

            Arac yeniArac = new Arac
            {
                Plaka = plaka,
                Marka = marka,
                KapasiteTon = kapasite,
                ModelYili = DateTime.Now.Year,
                AktifMi = true
            };

            if (!yeniArac.BilgilerGecerliMi())
            {
                MessageBox.Show("Araç bilgileri geçerli değil.");
                return;
            }

            _araclar.Add(yeniArac);

            PlakaTextBox.Clear();
            MarkaTextBox.Clear();
            KapasiteTextBox.Clear();

            KartlariGuncelle();
        }

        private void KartlariGuncelle()
        {
            ToplamAracText.Text = _araclar.Count.ToString();
            AktifAracText.Text =
                _araclar.Count(a => a.AktifMi).ToString();

            decimal toplamKapasite = _araclar.Sum(a => a.KapasiteTon);
            ToplamKapasiteText.Text = $"{toplamKapasite:N1} ton";
        }
    }
}
