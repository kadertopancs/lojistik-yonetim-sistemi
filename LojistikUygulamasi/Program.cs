using LojistikUygulamasi;

Console.WriteLine("locistik Uyhulamasi!");
Arac arac1 = new Arac();

arac1.KapasiteTon = 12.5m;
arac1.ModelYili = 2022;
arac1.Plaka = "31ABC123";

Console.WriteLine(arac1.BilgilerGecerliMi());
List<Arac> araclar = new List<Arac>();
araclar.Add(arac1);
Console.WriteLine("Listedeki araç sayısı: " + araclar.Count);
