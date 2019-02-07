using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanMitTextdateiSWSR
{
    class Program
    {
        static void HighscoreSpeichern(ref int Zeit, ref int Versuche)
        {
            StreamWriter DateiZeit = new StreamWriter(@"Zeit.txt", true);
            StreamWriter DateiVersuche = new StreamWriter(@"Versuche.txt", true);

            try
            {
                // Werte werden in die Datei gespeichert
                DateiZeit.WriteLine(Zeit);
                DateiVersuche.WriteLine(Versuche);
            }
            finally
            {
                DateiZeit.Close();
                DateiVersuche.Close();
            }
        }
        static void HighscoreAuslesen(ref int[] ZeitSpeichern, ref int[] VersucheSpeichern)
        {
            StreamReader DateiZeit = new StreamReader(@"Zeit.txt");
            StreamReader DateiVersuche = new StreamReader(@"Versuche.txt");
            string ZeitDaten;
            string VersucheDaten;
            int i = 0;

            try
            {
                while (DateiZeit.Peek() != -1)
                {
                    ZeitDaten = DateiZeit.ReadLine();
                    VersucheDaten = DateiVersuche.ReadLine();
                    // Arrays Größe wird geändert
                    if (i > ZeitSpeichern.Length - 1)
                        Array.Resize(ref ZeitSpeichern, ZeitSpeichern.Length + 1);
                    if (i > VersucheSpeichern.Length - 1)
                        Array.Resize(ref VersucheSpeichern, VersucheSpeichern.Length + 1);
                    try
                    {
                        ZeitSpeichern[i] = Convert.ToInt32(ZeitDaten);
                        VersucheSpeichern[i] = Convert.ToInt32(VersucheDaten);
                    }
                    catch
                    {
                        ZeitSpeichern[i] = 0;
                        VersucheSpeichern[i] = 0;
                    }
                    i++;
                }
            }
            finally
            {
                Array.Sort(ZeitSpeichern, VersucheSpeichern);
                DateiZeit.Close();
                DateiVersuche.Close();
            }
        }
        static void HighscoreAusgeben(ref int[] ZeitSpeichern, ref int[] VersucheSpeichern)
        {
            Console.WriteLine("Highscore\n---------");
            for (int i = 0; i <= 9; i++)
                Console.WriteLine("{0}. Platz: {1} Sekunden {2} Versuche", i + 1, ZeitSpeichern[i], VersucheSpeichern[i]);
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            Weiterspielen: // Für die goto-Anweisung
            // Deklaration von Variablen
            Random Zufall = new Random();
            string[] Wörter = new string[6] { "Peter", "Horst", "Thomas", "Helga", "Berta", "Julia" };
            string WortErraten = Wörter[Zufall.Next(0, Wörter.Length)];
            string Buchstaben = WortErraten.ToUpper();

            Stopwatch Stoppuhr = new Stopwatch(); // Stoppuhrfunktion
            TimeSpan Zeitspanne;
            int[] ZeitSpeichern = new int[10];
            int Zeit;
            int[] VersucheSpeichern = new int[10];
            int Versuche = 0;
            bool Gewonnen = false;
            int BuchstabenAnzeige = 0;
            string Eingabe;
            char Raten;

            StringBuilder Anzeige = new StringBuilder(WortErraten.Length); // Zeichen wird in Text umgewandelt
            // Klassen, um die Buchstabeneingaben zu sortieren
            List<char> RichtigGeraten = new List<char>();
            List<char> FalschGeraten = new List<char>();

            Stoppuhr.Start();
            for (int i = 0; i < WortErraten.Length; i++)
                Anzeige.Append("*"); // Für jede Buchstabe ein Zeichen
            Console.WriteLine(Anzeige);

            while (!Gewonnen) // Schleife um zu ermitteln, ob man Gewonnen hat
            {
                Console.Write("Bitte geben Sie einen Buchstaben ein: ");
                Eingabe = Console.ReadLine().ToUpper();
                Raten = Eingabe[0];
                Versuche++;
                // Es wird geprüft, ob die eingegebene Buchstabe richtig oder falsch ist
                if (Buchstaben.Contains(Raten))
                {
                    RichtigGeraten.Add(Raten);
                    for (int i = 0; i < WortErraten.Length; i++)
                    {
                        if (Buchstaben[i] == Raten)
                        {
                            Anzeige[i] = WortErraten[i];
                            BuchstabenAnzeige++;
                        }
                    }
                    if (BuchstabenAnzeige == WortErraten.Length)
                        Gewonnen = true;
                }
                else
                    FalschGeraten.Add(Raten);
                Console.WriteLine(Anzeige.ToString());
                // Bei Eingabe eines Sonderzeichens hat man die Möglichkeit das ganze Wort einzugeben
                if (Eingabe == "#")
                {
                    Console.Write("Bitte geben Sie das richtige Wort ein: ");
                    Eingabe = Console.ReadLine().ToUpper();
                    Raten = Eingabe[0];
                    if (Eingabe == Buchstaben)
                    {
                        Gewonnen = true;
                        Console.WriteLine(WortErraten);
                    }
                    else
                        continue; // Schleife kehrt zum Anfangspunkt zurück
                }
            }
            // Ausgabe
            if (Gewonnen)
            {
                Stoppuhr.Stop();
                Zeitspanne = Stoppuhr.Elapsed;
                Zeit = Zeitspanne.Seconds;
                Console.WriteLine("\n{0} Sekunden, {1} Versuche", Zeit, Versuche);
                HighscoreSpeichern(ref Zeit, ref Versuche);
            }
            Console.Write("\nWollen Sie weiterspielen ? (Ja = j/Nein = n): ");
            Eingabe = Console.ReadLine();
            if (Eingabe == "j")
                goto Weiterspielen; // wird direkt an die Marke übergeben
            else
            {
                HighscoreAuslesen(ref ZeitSpeichern, ref VersucheSpeichern);
                HighscoreAusgeben(ref ZeitSpeichern, ref VersucheSpeichern);
            }
        }
    }
}
