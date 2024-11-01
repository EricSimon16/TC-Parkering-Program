namespace TC_Parkering_Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Mesgun
            //Mesgun2
            //Eric
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
            }

            //test
            static void Ägare()
            {
                Console.WriteLine("Ägare");
            }
        }
    }
}
