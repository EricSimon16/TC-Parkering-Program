using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TC_Parkering_Program
{
    public class Program
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
                    KundUI(Parkering); // ÄNDRING: Anropar nya KundUI istället för Kund
                    break;
                case "2":
                    Vakt(Parkering);
                    break;
                case "3":
                    Ägare(Parkering);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

            await Parkering.VäntaTillsParkeringTom();
            Console.WriteLine("Alla parkeringsplatser är nu tomma.");
        }

        static void KundUI(parkeringsplats Parkering)
        {
            Console.WriteLine("Välkommen kund!");
            Console.WriteLine("Skriv in ditt registreringsnummer:");
            string regnummer = Console.ReadLine();

            Console.WriteLine("Skriv in färgen på din bil:");
            string färg = Console.ReadLine();

            Console.WriteLine("Välj fordonstyp:");
            Console.WriteLine("1: Bil");
            Console.WriteLine("2: Buss (Tar två platser)");
            Console.WriteLine("3: Motorcykel (Kan dela plats)");
            string fordonstypVal = Console.ReadLine();

            string fordonstyp = fordonstypVal switch
            {
                "1" => "bil",
                "2" => "buss",
                "3" => "motorcykel",
                _ => "bil" // Om användaren inte väljer ett giltigt alternativ
            };

            Console.WriteLine("Ange hur länge du vill parkera (i sekunder):");
            int parkeringstid;
            while (!int.TryParse(Console.ReadLine(), out parkeringstid) || parkeringstid <= 0)
            {
                Console.WriteLine("Vänligen ange ett giltigt antal sekunder.");
            }

            // Parkera fordonet (OBS! Ingen displayuppdatering här!)
            string plats = Parkering.ParkeraBil(regnummer, färg, fordonstyp, parkeringstid);
            Console.WriteLine($"Fordonet med registreringsnummer {regnummer} (typ: {fordonstyp}, färg: {färg}) har parkerat på: {plats}");

            // Lägger till ett alternativ för användaren att checka ut
            Console.WriteLine("\nVill du checka ut?");
            Console.WriteLine("1: Ja");
            Console.WriteLine("2: Nej");
            string kundVal = Console.ReadLine();

            if (kundVal == "1")
            {
                Console.WriteLine("Fordonet har checkat ut.");
                Parkering.CheckaUtBil(regnummer);

                // Efter att ha checkat ut, stäng applikationen
                Environment.Exit(0);
            }
            else if (kundVal == "2")
            {
                Console.WriteLine("Du har valt att stanna kvar.");
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
        }





        static void Vakt(parkeringsplats Parkering)
        {
            Console.WriteLine("Vakt");
            int antalBilar = new Random().Next(3, 11);
            Dictionary<string, decimal> bilKostnader = Parkering.GenereraBilarFörVakt(antalBilar);

            Console.WriteLine("Bilar genererade:");
            foreach (var bil in bilKostnader)
            {
                Console.WriteLine($"Bil {bil.Key}: Kostnad {bil.Value} SEK");
            }

            Console.WriteLine("\nSkriv in ett regnummer för att ge böter.");
            while (true)
            {
                string input = Console.ReadLine();
                Console.Clear();

                if (Parkering.GeBöter(input))
                {
                    Console.WriteLine($"Böter på 500 SEK {input}.");
                }
                else
                {
                    Console.WriteLine("Ogiltigt regnummer");
                }

                Console.WriteLine("\nlista med bilar:");
                Parkering.VisaListaMedBilar();
                Console.WriteLine($"\nTotala: {Parkering.HämtaTotalaIntäkter()} SEK");
            }

            Console.WriteLine($"\nTotala inkomster: {Parkering.HämtaTotalaIntäkter()} SEK");
        }

        static void Ägare(parkeringsplats Parkering)
        {
            Console.WriteLine("Ägare");
            for (int i = 0; i < 10; i++)
            {
                string parkering = Parkering.ParkeraBil();
                Console.WriteLine($"En ny bil parkerades på {parkering}.");
            }

            Console.WriteLine("Alla platser är nu fyllda");
        }

        public class parkeringsplats
        {

            public string GetFordonPåPlats(int platsIndex)
            {
                // Kollar om indexet är ett giltigt index (om det är mellan 0 och 9, eftersom du har 10 platser)
                if (platsIndex >= 0 && platsIndex < fordon.Length)
                {
                    return fordon[platsIndex];  // Returnera registreringsnumret för bilen på den platsen
                }
                return null;  // Om det inte finns något fordon på den platsen, returnera null
            }

            private bool ärTestmiljö;  // Ny flagga för att indikera om vi är i testmiljö

            public parkeringsplats(bool ärTestmiljö = false)
            {
                this.ärTestmiljö = ärTestmiljö;
            }



            private string[] fordon = new string[10];
            private string[] färger = new string[10]; // ÄNDRING: Ny array för att lagra färger
            private string[] typer = new string[10];  // ÄNDRING: Ny array för att lagra fordonstyp
            private int[] tider = new int[10];
            private List<string> utgångnaBilar = new List<string>();
            private Random random = new Random();
            private object låsObjekt = new object();
            private int aktivaBilar = 0;
            private decimal dagsinkomst = 0m;
            private const decimal böter = 500m;


            public string ParkeraBil(string regnummer, string färg, string typ, int tid)
            {
                lock (låsObjekt)
                {
                    if (typ == "buss")
                    {
                        // Leta efter två lediga platser i rad
                        for (int i = 0; i < fordon.Length - 1; i++) // -1 för att undvika indexfel
                        {
                            if (fordon[i] == null && fordon[i + 1] == null) // Kontrollera två platser i rad
                            {
                                // Markera båda platserna för bussen
                                fordon[i] = regnummer;
                                fordon[i + 1] = regnummer; // Samma registreringsnummer för båda
                                färger[i] = färg;
                                färger[i + 1] = färg;
                                typer[i] = typ;
                                typer[i + 1] = typ;
                                tider[i] = tid;
                                tider[i + 1] = tid; // Samma tid för båda platserna
                                Interlocked.Increment(ref aktivaBilar);
                                Interlocked.Increment(ref aktivaBilar); // Bussen tar två platser
                                StartaNedräkning(i);
                                StartaNedräkning(i + 1);
                                UppdateraDisplay();
                                return $"Plats {i + 1} och {i + 2}";
                            }
                        }
                        return "Det finns inga två sammanhängande lediga parkeringsplatser för en buss.";
                    }
                    else
                    {
                        // Ursprunglig logik för bil/motorcykel
                        for (int i = 0; i < fordon.Length; i++)
                        {
                            if (fordon[i] == null)
                            {
                                fordon[i] = regnummer;
                                färger[i] = färg;
                                typer[i] = typ;
                                tider[i] = tid;
                                Interlocked.Increment(ref aktivaBilar);
                                StartaNedräkning(i);
                                UppdateraDisplay();
                                return $"Plats {i + 1}";
                            }
                        }
                        return "Det finns inga lediga parkeringsplatser.";
                    }
                }
            }


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
                const string siffror = "0123456789";
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
                    färger[plats] = null; // ÄNDRING: Rensa färg
                    typer[plats] = null;  // ÄNDRING: Rensa fordonstyp
                    tider[plats] = 0;
                    Interlocked.Decrement(ref aktivaBilar);
                    dagsinkomst += böter;
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
                if (ärTestmiljö)
                {
                    // Hoppa över att uppdatera displayen i testmiljö
                    return;
                }

                lock (låsObjekt)
                {
                    Console.Clear();  // Detta orsakar felet i testmiljö
                    Console.WriteLine("Parkeringsstatus:");

                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] != null)
                        {
                            Console.WriteLine($"Plats {i + 1}: {fordon[i]} - Tid: {tider[i]} sekunder");
                        }
                        else
                        {
                            Console.WriteLine($"Plats {i + 1}: Ledig");
                        }
                    }

                    Console.WriteLine("\nBilar parkeringstid är 0:");
                    foreach (var bil in utgångnaBilar)
                    {
                        Console.WriteLine($"Bil: {bil}");
                    }

                    Console.WriteLine($"\nDagens intäkter: {dagsinkomst} SEK");
                }
            }
        



        public Dictionary<string, decimal> GenereraBilarFörVakt(int antal)
            {
                Dictionary<string, decimal> genereradeBilar = new Dictionary<string, decimal>();

                for (int i = 0; i < antal; i++)
                {
                    string bilnummer = GenereraReggnummer();
                    int tid = random.Next(10, 61);
                    decimal kostnad = tid * 1.5m;
                    genereradeBilar[bilnummer] = kostnad;

                    lock (låsObjekt)
                    {
                        fordon[i] = bilnummer;
                        tider[i] = tid;
                        dagsinkomst += kostnad;
                    }
                }

                return genereradeBilar;
            }

            public bool GeBöter(string regnummer)
            {
                lock (låsObjekt)
                {
                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] == regnummer)
                        {
                            fordon[i] = null;
                            tider[i] = 0;
                            dagsinkomst += böter;
                            return true;
                        }
                    }
                }
                return false;
            }

            public decimal HämtaTotalaIntäkter()
            {
                return dagsinkomst;
            }

            internal void CheckaUtBil(string regnummer)
            {
                lock (låsObjekt)
                {
                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] == regnummer)
                        {
                            // Ta bort bilen från parkeringsplatsen
                            fordon[i] = null;
                            färger[i] = null;  // Rensa färg
                            typer[i] = null;   // Rensa fordonstyp
                            tider[i] = 0;      // Rensa parkeringstid

                            // Minska antalet aktiva bilar
                            Interlocked.Decrement(ref aktivaBilar);

                            // Uppdatera parkeringsstatusen
                            UppdateraDisplay();

                            Console.WriteLine($"Bilen med registreringsnummer {regnummer} har checkat ut.");
                            return;  // Avsluta när bilen är borttagen
                        }
                    }

                    // Om bilen inte hittades
                    Console.WriteLine($"Bilen med registreringsnummer {regnummer} finns inte på parkeringen.");
                }
            }

            public void VisaListaMedBilar()
            {
                lock (låsObjekt)
                {
                    Console.WriteLine("Bilar på parkeringen:");
                    for (int i = 0; i < fordon.Length; i++)
                    {
                        if (fordon[i] != null)
                        {
                            Console.WriteLine($"Plats {i + 1}: {fordon[i]} - Tid: {tider[i]} sekunder");
                        }
                    }
                }
            }
        }
    }
}