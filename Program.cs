using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TC_Parkering_Program
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string användarval;
            Console.WriteLine("Välj din roll");
            Console.WriteLine("1: Kund");
            Console.WriteLine("2: Parkeringsvakt");
            Console.WriteLine("3: Ägare");
            användarval = Console.ReadLine();

            parkeringsplats Parkering = new parkeringsplats();

            switch (användarval)
            {
                case "1":
                    Kund(Parkering);
                    break;
                case "2":
                    Vakt(Parkering);
                    break;
                case "3":
                    Ägare();
                    break;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

            await Parkering.VäntaTillsParkeringTom();
            Console.WriteLine("Alla parkeringsplatser är nu tomma. Programmet avslutas.");
        }

        static void Kund(parkeringsplats Parkering)
        {
            Console.WriteLine("Välkommen kund, vänligen parkera på en ledig plats.");
            string bilnummer = string.Empty;

            while (string.IsNullOrWhiteSpace(bilnummer))
            {
                bilnummer = Console.ReadLine();
            }

            string parkering = Parkering.ParkeraBil(bilnummer);
            Console.WriteLine($"Din bil ({bilnummer}) har parkerat på plats {parkering}.");

            for (int i = 0; i < 4; i++)
            {
                Parkering.ParkeraBil();
            }
        }

        static void Vakt(parkeringsplats Parkering)
        {
            Console.WriteLine("Vakt");
            for (int i = 0; i < 10; i++)
            {
                Parkering.ParkeraBil();
            }
        }

        static void Ägare()
        {
            Console.WriteLine("Ägare");
        }

        public class parkeringsplats
        {
            private string[] fordon = new string[10];
            private int[] tider = new int[10];
            private List<string> utgångnaBilar = new List<string>();  // För att spara vilka bilar som har gått ut i en list
            private Random random = new Random();
            private object låsObjekt = new object();
            private int aktivaBilar = 0;

            public string ParkeraBil(string bilnummer = null)
            {
                if (string.IsNullOrEmpty(bilnummer))
                {
                    bilnummer = GenereraReggnummer();
                }

                lock (låsObjekt)
                {
                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] == null)
                        {
                            fordon[i] = bilnummer;
                            tider[i] = random.Next(10, 61);
                            Interlocked.Increment(ref aktivaBilar);
                            StartaNedräkning(i);
                            UppdateraDisplay();
                            return $"Plats {i + 1}";
                        }
                    }
                }
                return "Det finns inga lediga parkeringsplatser.";
            }

            private string GenereraReggnummer()
            {
                const string bokstäver = "ABCDEFGHIJKLMNOPQRSTUVWHYZ";
                const string siffror = "012345678";
                char[] reg = new char[6];
                for (int i = 0; i < 3; i++)
                {
                    reg[i] = bokstäver[random.Next(bokstäver.Length)];
                }
                for (int i = 3; i < 6; i++)
                {
                    reg[i] = siffror[random.Next(siffror.Length)];
                }
                return new string(reg);
            }

            private void StartaNedräkning(int plats)
            {
                Task.Run(async () =>
                {
                    while (tider[plats] > 0)
                    {
                        await Task.Delay(1000);
                        tider[plats]--;
                        UppdateraDisplay();
                    }
                    BilGåttUt(plats);
                    UppdateraDisplay();
                });
            }

            private void BilGåttUt(int plats)
            {
                lock (låsObjekt)
                {
                    utgångnaBilar.Add(fordon[plats]);
                    fordon[plats] = null;
                    tider[plats] = 0;
                    Interlocked.Decrement(ref aktivaBilar);
                }
            }

            public async Task VäntaTillsParkeringTom()
            {
                while (aktivaBilar > 0)
                {
                    await Task.Delay(1000);
                }
            }

            private void UppdateraDisplay()
            {
                lock (låsObjekt)
                {
                    Console.Clear();
                    Console.WriteLine("Parkeringsstatus:");

                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] != null)
                        {
                            Console.WriteLine($"Plats {i + 1}: {fordon[i]} - Tid kvar: {tider[i]} sekunder");
                        }
                        else
                        {
                            Console.WriteLine($"Plats {i + 1}: Ledig");
                        }
                    }

                    Console.WriteLine("\nBilar vars parkeringstid har gått ut:");
                    foreach (var bil in utgångnaBilar)
                    {
                        Console.WriteLine($"Bil: {bil}");
                    }
                }
            }
        }
    }
}