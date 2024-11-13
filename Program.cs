using System;

namespace TC_Parkering_Program
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string användarval;
            Console.WriteLine("Välj din roll");
            Console.WriteLine("1: Kund");
            Console.WriteLine("2: Parkeringsvakt");
            Console.WriteLine("3: Ägare");
            användarval = Console.ReadLine();
            switch (användarval)
            {
                case "1":
                    Kund();
                    break;

                case "2":
                    Vakt();
                    break;

                case "3":
                    Ägare();
                    break;

                default:
                    Console.WriteLine("Ogiltigt");
                    break;
            }
            // mesgun....
            static void Kund()
            {
                Console.WriteLine("Välkommen kund, vänligen parkera på en ledig plats.");
                string bilnummer = string.Empty;

                //
                while (string.IsNullOrWhiteSpace(bilnummer))
                {
                    bilnummer = Console.ReadLine();
                }

                parkeringsplats Parkering = new parkeringsplats();
                string parkering = Parkering.parkeraBil(bilnummer);

                //
                Console.WriteLine($"{parkering}");


                for (int i = 0; i < 10; i++)
                {
                    string parkering1 = Parkering.parkeraBil();
                    Console.WriteLine(parkering1);
                }
            }

            static void Vakt()
            {
                Console.WriteLine("Vakt");

                parkeringsplats Parkering = new parkeringsplats();

                for (int i = 0; i < 10; i++)
                {
                    string parkering = Parkering.parkeraBil();
                    Console.WriteLine(parkering);
                }
            }

            static void Ägare()
            {
                Console.WriteLine("Ägare");
            }
        }

        public class parkeringsplats
        {

            public string[] fordon = new string[25]; //

            private Random random = new Random();


            public string Reggnummer()
            {
                const string bokstäver = "ABCDEFGHIJKLMNOPQRSTUVWHYZ";
                const string Siffror = "012345678";

                char[] Reg = new char[6];
                for (int i = 0; i < 3; i++)
                {
                    Reg[i] = bokstäver[random.Next(bokstäver.Length)];
                }

                for (int i = 3; i < 6; i++)
                {
                    Reg[i] = Siffror[random.Next(Siffror.Length)];
                }

                return new string(Reg);
            }


            public string parkeraBil(string bilnummer = null)
            {
                if (string.IsNullOrEmpty(bilnummer))
                {
                    bilnummer = Reggnummer();
                }

                for (int i = 0; i < fordon.Length; i++)
                {
                    if (fordon[i] == null)
                    {
                        fordon[i] = bilnummer;  //
                        return $"{i + 1}: {bilnummer}";  //
                    }
                }

                return "Det finns inga lediga parkeringsplatser.";
            }
        }

        public class vehicle
        {
            public string Name { get; set; } = "John";
            public int numberOfWheels { get; set; }
            

        }
        public class car : vehicle
        {
            public string size { get; set; }

        }
        public class bus : vehicle
        {
            public int seats { get; set; }
        }
        public class motorcycle : vehicle
        {
            public int size { get; set; }
        }
    }
}
