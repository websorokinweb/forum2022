using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{
    internal class Helpers
    {
        public double SaveUserDouble(string message, string errorMessage)
        {
            double correctUserInput;
            string userInput;

            Console.Write(message + " ");
            userInput = Console.ReadLine();
            while (!Double.TryParse(userInput, out correctUserInput))
            {
                Console.Write(errorMessage + " ");
                userInput = Console.ReadLine();
            }
            return correctUserInput;
        }

        public int SaveUserInt(string message, string errorMessage)
        {
            int correctUserInput;
            string userInput;

            Console.Write(message + " ");
            userInput = Console.ReadLine();
            while (!int.TryParse(userInput, out correctUserInput))
            {
                Console.Write(errorMessage + " ");
                userInput = Console.ReadLine();
            }
            return correctUserInput;
        }

        public string SaveUserStr(string message, string errorMessage)
        {
            string userInput;

            Console.Write(message + " ");
            userInput = Console.ReadLine();
            return userInput;
        }
    }
}
