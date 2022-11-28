using System;

namespace forum2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Auth.SetCurrentUser();
            Auth.ShowCurrentUser();
            Menu.InitMenu();
        }
    }
}
