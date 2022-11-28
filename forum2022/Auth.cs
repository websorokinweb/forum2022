using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{
    struct User
    {
        public string username;
        public string password;
    }

    public class Auth
    {
        static User currentUser = new User();

        public static void SetCurrentUser()
        {
            currentUser.username = "test";
            currentUser.password = "pass";
        }

        public static void ShowCurrentUser()
        {
            Console.WriteLine(currentUser.username);
            Console.WriteLine(currentUser.password);
        }
    }
}
