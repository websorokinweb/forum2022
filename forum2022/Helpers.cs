using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{
    internal class Helpers
    {
        public static bool validateFalse(string userInput){
            return false;
        }
        public static double SaveUserDouble(string message, string errorMessage)
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

        public static int SaveUserInt(string message, string errorMessage)
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

        public static string SaveUserStr(string message, Func<string, bool> validateString)
        {
            string userInput;

            while(true){
                Console.Write(message + " ");
                userInput = Console.ReadLine();
                if(validateString(userInput)){
                    return userInput;
                }

            }
        }
    }
}
