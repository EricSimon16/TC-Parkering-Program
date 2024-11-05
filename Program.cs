using System;
using System.Security.Cryptography.X509Certificates;

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



            static void Kund()
            {
                Console.WriteLine("Kund");
            }


            static void Vakt()
            {

                Console.WriteLine("Vakt");
                Regg Bilnummer = new Regg();
                string bilnummer = Bilnummer.Reggnummer();
                Console.WriteLine(bilnummer);

            }


            static void Ägare()
            {
                Console.WriteLine("Ägare");
            }


        }

        public class ParkeringsPlater
        {
            public string[] ParkeringPlats;


        }

        public class Regg
        {
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




        }
    }
}
