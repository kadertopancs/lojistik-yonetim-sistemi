
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace LojistikUygulamasi.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Arac> _araclar = new();

        private readonly string _connectionString =
            @"Server=localhost\SQLEXPRESS;Database=LojistikDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public MainWindow()
        {
            InitializeComponent();

            AraclarDataGrid.ItemsSource = _araclar;

            AraclariYukle();
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

            try
            {
                using SqlConnection connection =
                    new SqlConnection(_connectionString);

                connection.Open();

                string sql = @"
                    INSERT INTO Araclar
                        (Plaka, Marka, KapasiteTon, AktifMi, ModelYili)
                    VALUES
                        (@Plaka, @Marka, @KapasiteTon, @AktifMi, @ModelYili)";

                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Plaka", yeniArac.Plaka);
                command.Parameters.AddWithValue("@Marka", yeniArac.Marka);
                command.Parameters.AddWithValue("@KapasiteTon", yeniArac.KapasiteTon);
                command.Parameters.AddWithValue("@AktifMi", yeniArac.AktifMi);
                command.Parameters.AddWithValue("@ModelYili", yeniArac.ModelYili);

                command.ExecuteNonQuery();

                _araclar.Add(yeniArac);

                PlakaTextBox.Clear();
                MarkaTextBox.Clear();
                KapasiteTextBox.Clear();

                KartlariGuncelle();

                MessageBox.Show("Araç veritabanına kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Araç kaydedilemedi:\n" + ex.Message,
                    "Veritabanı hatası");
            }
        }

        private void AraclariYukle()
        {
            try
            {
                using SqlConnection connection =
                    new SqlConnection(_connectionString);

                connection.Open();

                string sql = @"
                    SELECT AracId, Plaka, Marka, KapasiteTon, AktifMi, ModelYili
                    FROM Araclar";

                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _araclar.Add(new Arac
                    {
                        AracId = reader.GetInt32(0),
                        Plaka = reader.GetString(1),
                        Marka = reader.GetString(2),
                        KapasiteTon = reader.GetDecimal(3),
                        AktifMi = reader.GetBoolean(4),
                        ModelYili = reader.GetInt32(5)
                    });
                }

                KartlariGuncelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Araçlar yüklenemedi:\n" + ex.Message,
                    "Veritabanı bağlantı hatası");
            }
        }

        private void KartlariGuncelle()
        {
            ToplamAracText.Text = _araclar.Count.ToString();

            AktifAracText.Text =
                _araclar.Count(a => a.AktifMi).ToString();

            decimal toplamKapasite =
                _araclar.Sum(a => a.KapasiteTon);

            ToplamKapasiteText.Text = $"{toplamKapasite:N1} ton";
        }
    }
}
