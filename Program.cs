// See https://aka.ms/new-console-template for more information
using System;
using static Kino;

Kino salaKinowa = new Kino(5,10);
Menu mojMenu = new Menu();
UsuwanieRezerwacji usunMiejsce = new UsuwanieRezerwacji(@"C:\Users\Wojtek\source\repos\eksperyment\eksperyment\", "Zarezerwowane_miejsca_w_kinie.txt");
    ;
int opcja;
opcja = mojMenu.Wyswietl();


switch (opcja)
{
    case 1:
        {          
            Console.WriteLine($"Wybrałeś opcje {opcja}");
            Console.WriteLine("Podaj imie");
            string imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko");
            string nazwisko = Console.ReadLine();
            Console.WriteLine("Podaj numer telefonu");
            int numerTelefonu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Ile miejsc chesz zajac:");
            int liczbaMiejsc = Convert.ToInt32(Console.ReadLine()); 
            salaKinowa.PodajMiejsce(liczbaMiejsc,imie,nazwisko,numerTelefonu);
            salaKinowa.RozpsujSaleKinowa();
            break;
        }
    case 2:
        {
            Console.WriteLine($"Wybrałeś opcje {opcja}");
            salaKinowa.RozpsujSaleKinowa();           
            break;
        }
    case 3:
        {
            Console.WriteLine("Wybrałeś opcje 3");
            Console.WriteLine("Podaj imie");
            string imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko");
            string nazwisko = Console.ReadLine();
            Console.WriteLine("Podaj numer telefonu");
            int numerTelefonu = Convert.ToInt32(Console.ReadLine());
            usunMiejsce.WyszukajZarezerwowaneMiejsca(imie, nazwisko, numerTelefonu);
            Console.WriteLine();
            break;
        }
    default:
        {
            Console.WriteLine("Błąd");
            mojMenu.Wyswietl();
            break;
        }
}
class Menu
{
    public int Wyswietl()
    {
        int opcja;
        string jeden = "1. Zarezerwuj miejsce";
        string dwa = "2. Sprawdz wolne miejsca";
        string trzy = "3. Usun rezerwacje ";
        Console.WriteLine($"Menu Kina\n{jeden}\n{dwa}\n{trzy}");
        Console.WriteLine();
        Console.WriteLine("Wpisz 1 by wybrac pierwsza opcje ,2 by wybrac druga lub 3 trzecią");
        opcja = Convert.ToInt32(Console.ReadLine());
        return opcja;
    }
}
class miejsceWKinie
{
   private bool wolne;
    public miejsceWKinie(bool stan = true)
    {
        wolne = stan;
    }
   public void ZmienStanMiejsca(bool stan)
    {
        wolne= stan;
    }
    public bool PobierzStanMiejsca()
    {
        return wolne;
    }
}
class Kino
{
    ZapisRezerwacji zapisRezerwacji = new ZapisRezerwacji(@"C:\Users\Wojtek\source\repos\eksperyment\eksperyment\", "Zarezerwowane_miejsca_w_kinie.txt");
    public int Rzedy { get; }
    public int Kolumny { get; }
    public int MiejscaWolne;
    public int MiejscaZajete;
    public miejsceWKinie[,] Miejsca { get; }
    public Kino( int rzedy, int kolumny)
    {

        Rzedy = rzedy;
        Kolumny = kolumny;
        Miejsca = new miejsceWKinie[Rzedy, Kolumny];
        for (int i = 0; i < Rzedy; i++)
        {
            for(int j = 0; j < Kolumny; j++)
            {

                Miejsca[i, j] = new miejsceWKinie();
            }
        }
        ZapisywanieRezerwacjiWKinieZPLiku();
     }
    private void ZapisywanieRezerwacjiWKinieZPLiku()
    {
        var zajeteMiejsca = zapisRezerwacji.ZapisywanieRezerwaacjiZPliku();
        zajeteMiejsca.ForEach(item =>
        {
            Miejsca[item[0] - 1, item[1] - 1].ZmienStanMiejsca(false);
        });
    }
    public void RozpsujSaleKinowa()
    {
        Console.WriteLine("---EKRAN---");
        for (int i = 0; i < Rzedy; i++)
        {
            for (int j = 0; j < Kolumny; j++)
            {
                if (Miejsca[i, j].PobierzStanMiejsca() == true)
                {
                    Console.Write("O");
                    MiejscaWolne++;
                }
                else
                {
                    Console.Write("X");
                    MiejscaZajete++;
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Miejsc wolnych jest {MiejscaWolne}");
        Console.WriteLine($"Mejsc zajestych jest {MiejscaZajete}");
    }

   public void PodajMiejsce(int ileMiejsc, string imie, string nazwisko, int numerTelefonu)
    {
        for (int i = 0; i < ileMiejsc; i++)
        {
            Console.WriteLine("Podaj rzad  ktory chcesz zajac");
            int rzad = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj kolumne miejsa ktory chcesz zajac");
            int kolumna = Convert.ToInt32(Console.ReadLine());
            if (rzad <= 5 && kolumna <= 10)
            {
                if (Miejsca[rzad-1, kolumna-1].PobierzStanMiejsca() == true)
                {
                    Miejsca[rzad-1, kolumna-1].ZmienStanMiejsca(false);
                    zapisRezerwacji.RezerwacjaMiejsca(imie, nazwisko, numerTelefonu,rzad,kolumna);
                    Console.WriteLine("Zarezerwuj kolejne miejsce");
                }
                else
                {
                    Console.WriteLine("Miejsce juz zajete");
                }
            }
            else
            {
                Console.WriteLine("blad, nie ma taiego miejsca");
            }
        }
    }
}
public class ZapisRezerwacji
{
    public string NazwaPliku { get; set; }
    public string ScieżkaDostepu { get; set; }
   public ZapisRezerwacji(string podanaSciezka, string nazwaPliku)
    {
        NazwaPliku = nazwaPliku;
        ScieżkaDostepu = podanaSciezka;
    }
    public void RezerwacjaMiejsca(string imie, string nazwisko, int numer, int rzad, int kolumna)
    {
        string sciezka = ScieżkaDostepu + NazwaPliku;
        using (StreamWriter plik = File.AppendText(sciezka))
        {
            if (File.Exists(sciezka))
            {
                string wiersz = imie + "," + nazwisko + "," + numer + ","+rzad +","+kolumna;
                plik.WriteLine(wiersz);
            }
            else
            {
                Console.WriteLine("Brak takiej książki");
            }
        }
    }
    public List<int[]> ZapisywanieRezerwaacjiZPliku()
    {
        string sciezka = ScieżkaDostepu + NazwaPliku;
        var wynik = File.ReadAllLines(sciezka);
        var ZajeteMiejscaZpliku = new List<int[]>();
        foreach (var item in wynik)
        {
            var DaneZWiersza = item.Split(',');
            int[] miejsca = new int[2] { int.Parse(DaneZWiersza[3]), int.Parse(DaneZWiersza[4]) };
            ZajeteMiejscaZpliku.Add(miejsca);
        }
        return ZajeteMiejscaZpliku;
    }
}
 class UsuwanieRezerwacji
{
    public string NazwaPliku { get; set; }
    public string SciezkaDstepu { get; set; }
   public UsuwanieRezerwacji(string sciezkaDostepu, string nazwaPliku) 
    {
        NazwaPliku = nazwaPliku;
        SciezkaDstepu = sciezkaDostepu;
    }
    public void WyszukajZarezerwowaneMiejsca(string imie, string nazwisko, int numerTelefonu)
    {
        string sciezka = SciezkaDstepu + NazwaPliku;
        var wiersze = File.ReadAllLines(sciezka);
        foreach (var item in wiersze)
        {
            var DaneZWiersza = item.Split(',');
            if (DaneZWiersza[0] == imie && DaneZWiersza[1] == nazwisko && DaneZWiersza[2] == numerTelefonu.ToString())
            {
                Console.WriteLine("imie: " + imie + " nazwisko: " + nazwisko + " numer telefonu: " + numerTelefonu + " rząd: " + DaneZWiersza[3] + " kolumna: " + DaneZWiersza[4]);
            }
        }
        Console.WriteLine("Podaj liczbe miejsc ile chcesz usunac");
        int ileMiejsc = Convert.ToInt32(Console.ReadLine());
        for(int i = 0; i < ileMiejsc; i++)
        {
            Console.WriteLine("Podaj rzad ktory chcesz usunac");
            int podanyRzad = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj kolumne ktora chcesz usunac");
            int podanaKolumna = Convert.ToInt32(Console.ReadLine());
            PobieranieDaychZPliku(imie, nazwisko, numerTelefonu, podanyRzad, podanaKolumna);
        }
        Console.WriteLine("Usunieto");
    }
    public void PobieranieDaychZPliku(string imie, string nazwisko, int numerTelefonu, int rzad, int kolumna)
    {
        string sciezka = SciezkaDstepu + NazwaPliku;
        var daneZPliku = File.ReadAllLines(sciezka);
       var daneDoUsuniecia = new string[] { imie + "," + nazwisko + "," + numerTelefonu + "," + rzad + "," + kolumna };
        var ZedytowaneDaneDoPliku = daneZPliku.Except(daneDoUsuniecia).ToList();
        File.WriteAllLines(sciezka, ZedytowaneDaneDoPliku);
       
    }

}


