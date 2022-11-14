using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{
    public struct User
    {
        public static string username;
        public static string password;
    }

    

    public class Auth
    {
        public static User currentUser;

        public static void SetCurrentUser()
        {

        }

        public static void ShowCurrentUser()
        {
            Console.WriteLine(currentUser);
            User.username = "test";
            Console.WriteLine(currentUser);
        }
    }
}
