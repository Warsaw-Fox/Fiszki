using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int punkty = 0;
        string jezyk = "";
        // Główna pętla programu
        while (true)
        {
            if (string.IsNullOrEmpty(jezyk) || jezyk == "wybor_jezyka")
            {
                jezyk = WyborJezyka(punkty);
                continue; // Wrócimy na początek pętli.
            }

            string poziom = WyborPoziomu(jezyk, punkty);

            if (poziom == "wybor_jezyka")
            {
                jezyk = poziom;
                continue; // Wracaj do wyboru języka.
            }
            // Pętla dla wybranego poziomu
            while (true)
            {
                string plikSlowka = $"{jezyk}_{poziom}_slowka.csv";
                string plikZdania = $"{jezyk}_{poziom}_zdania.csv";

                var listaSlowek = WczytajCSV(plikSlowka);
                var listaZdan = WczytajCSV(plikZdania);

                string tryb = WyborTrybu();

                if (tryb == "powrot_do_poziomu")
                {
                    break; // Przerwij wewnętrzne while i wróć do wyboru poziomu.
                }

                switch (tryb)
                {
                    case "nauka":
                        punkty = NaukaSłówek(listaSlowek, punkty);
                        break;
                    case "wstawianie":
                        punkty = WstawianieSłówek(listaZdan, punkty);
                        break;
                }
            }
        }
    }
    // Funkcja do wyboru języka
    static string WyborJezyka(int punkty)
    {
        while (true)
        {
            Console.WriteLine("Wybierz język (1. angielski, 2. francuski, 3. zakończ program 4.ranking)");
            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    return "angielski";
                case "2":
                    return "francuski";
                case "3":
                    ZapiszDoRankingu(punkty);  // Tutaj korzystamy z punktów zdobytych przez czas działania prgoramu orginalnie wybór języka resetował punkty ale zrezygnowan z tego.
                    Environment.Exit(0); // Zamyka program
                    return "";
                case "4":
                    PokazRanking();
                    return "wybor_jezyka"; 
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    continue;
            }
        }
    }
    // Funkcja do wyboru poziomu trudności
    static string WyborPoziomu(string jezyk, int punkty)
    {
        while (true)
        {
            Console.WriteLine("Wybierz poziom (1. latwy, 2. sredni, 3. trudny, 4. powrót do wyboru języka):");
            string wyborPoziomu = Console.ReadLine();

            switch (wyborPoziomu)
            {
                case "1":
                    return "latwy";
                case "2":
                    return "sredni";
                case "3":
                    return "trudny";
                case "4":
                    return "wybor_jezyka"; // Specjalny przypadek, który będzie obsługiwany w Main.
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    continue;
            }
        }
    }
    // Funkcja do wyboru trybu nauki
    static string WyborTrybu()
    {
        while (true)
        {
            Console.WriteLine("Wybierz tryb (1. nauka, 2. wstawianie, 3. powrót do wyboru poziomu):");
            string wyborTrybu = Console.ReadLine();

            switch (wyborTrybu)
            {
                case "1":
                    return "nauka";
                case "2":
                    return "wstawianie";
                case "3":
                    return "powrot_do_poziomu";
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    continue;
            }
        }
    }
    // Funkcja do nauki słówek
    static int NaukaSłówek(List<string[]> listaSlowek, int punkty)
    {
        // Wyczyść ekran
        Console.Clear();
        Random rnd = new Random();
        int poprawneOdpowiedziZRzedu = 0;
        for (int i = 0; i < 10; i++)
        {
            int losowyIndex = rnd.Next(listaSlowek.Count-1);
            var element = listaSlowek[losowyIndex];

            // Debugowanie: Wyświetlenie zawartości elementu
           // Console.WriteLine($"DEBUG: element = [{string.Join(", ", element)}]");

            Console.WriteLine($"Twój aktualny wynik: {punkty}");
            Console.WriteLine($"Przetłumacz: {element[1]}");
            string odpowiedz = Console.ReadLine().Trim();

            if (odpowiedz == element[0])
            {
                Console.WriteLine("Dobrze!");
                punkty++;
                poprawneOdpowiedziZRzedu++;

                if (poprawneOdpowiedziZRzedu == 3)
                {
                    Console.WriteLine("Gratulacje! Otrzymujesz dodatkowy punkt za 3 poprawne odpowiedzi z rzędu!");
                    punkty++;
                    poprawneOdpowiedziZRzedu = 0;  // resetujemy licznik
                }

                // Odczekaj 2 sekundy
                Thread.Sleep(2000);

                // Wyczyść ekran
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"Błąd! Prawidłowa odpowiedź to: {element[0]}");
                poprawneOdpowiedziZRzedu = 0;  // resetujemy licznik przy błędnej odpowiedzi
                                               // Odczekaj 2 sekundy
                Thread.Sleep(2000);

                // Wyczyść ekran
                Console.Clear();
            }
        }

        Console.WriteLine($"Twój wynik: {punkty}");
        // Odczekaj 2 sekundy
        Thread.Sleep(2000);

        // Wyczyść ekran
        Console.Clear();
        return punkty;
    }
    // Funkcja do wstawiania słówek w zdaniach
    static int WstawianieSłówek(List<string[]> listaZdan, int punkty)
    {
        Console.Clear();
        Random rnd = new Random();
        int poprawneOdpowiedziZRzedu = 0;
        for (int i = 0; i < 10; i++)
        {
            int losowyIndex = rnd.Next(listaZdan.Count-1);
            var element = listaZdan[losowyIndex];

            Console.WriteLine($"Twój aktualny wynik: {punkty}");
            Console.WriteLine($"Wstaw brakujące słowo: {element[0]}");
            string odpowiedz = Console.ReadLine().Trim();

            if (odpowiedz == element[1])
            {
                Console.WriteLine("Dobrze!");
                punkty++;
                poprawneOdpowiedziZRzedu++;

                if (poprawneOdpowiedziZRzedu == 3)
                {
                    Console.WriteLine("Gratulacje! Otrzymujesz dodatkowy punkt za 3 poprawne odpowiedzi z rzędu!");
                    punkty++;
                    poprawneOdpowiedziZRzedu = 0;  // resetujemy licznik
                }
                Thread.Sleep(2000);

                // Wyczyść ekran
                Console.Clear();

            }
            else
            {
                Console.WriteLine($"Błąd! Prawidłowa odpowiedź to: {element[1]}");
                poprawneOdpowiedziZRzedu = 0;  // resetujemy licznik przy błędnej odpowiedzi
                Thread.Sleep(2000);

                // Wyczyść ekran
                Console.Clear();
            }
        }

        Console.WriteLine($"Twój wynik: {punkty}");
        return punkty;
        // Powyższa funkcja jest podobna do NaukaSłówek, ale zamiast tłumaczenia, użytkownik wstawia brakujące słowo w zdaniu dzięki czemu uczy się rzoumieć kontekst użycia ich
    }

    // Funkcja do wczytania danych z pliku CSV
    static List<string[]> WczytajCSV(string nazwaPliku)
    {
        var lista = new List<string[]>();
        var linie = File.ReadAllLines(nazwaPliku);

        foreach (var linia in linie)
        {
            lista.Add(linia.Split(','));
        }

        return lista;
        // Wczytuje dane z pliku CSV i zwraca je jako listę tablic stringów.
    }
    // Funkcja do zapisywania punktów do rankingu
    static void ZapiszDoRankingu(int punkty)
    {
       // Console.WriteLine($"Próba zapisu {punkty} punktów do rankingu..."); // Debug
        if (punkty > 0)
        {
            Console.WriteLine("Wprowadź swój nick:");
            string nick = Console.ReadLine();

            try
            {
                using (StreamWriter sw = File.AppendText("ranking.csv"))
                {
                    sw.WriteLine($"{punkty},{nick}");
                }
                Console.WriteLine("Zapisano do rankingu."); // Debug
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas zapisywania do rankingu: " + ex.Message); // Debug
            }
        }
        else
        {
            Console.WriteLine("Brak punktów do zapisania."); // Debug
        }
    }

    // Funkcja do wyświetlania rankingu
    static void PokazRanking()
    {
        if (File.Exists("ranking.csv"))
        {
            var linie = File.ReadAllLines("ranking.csv");
            Console.WriteLine("RANKING:");
            foreach (var linia in linie)
            {
                Console.WriteLine(linia);
            }
        }
        else
        {
            Console.WriteLine("Ranking jest pusty.");
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić...");
        Console.ReadKey();
    }
}

