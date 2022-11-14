using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{
    public class Menu
    {
        public static bool isListening = true;

        public static void InitMenu()
        {
            Console.WriteLine("Menu inited");
            while (isListening)
            {
                int operationType;

                operationType = Helpers.SaveUserInt("Wpisz liczbę", "Nieprawidlowa liczba");

                switch (operationType)
                {
                    case 0:
                        isListening = false;
                        break;

                    case 1:
                        
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Nieprawidlowy typ operacji. Napisz liczbe od 1 do 4, lub 0 dla wyjścia");
                        break;
                }
            }
        }
    }
}
