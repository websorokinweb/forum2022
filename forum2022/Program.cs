using System;

namespace forum2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Auth.SetCurrentUser();
            Auth.ShowCurrentUser();
            Auth.ShowAllUsers();
            Menu.InitMenu();
        }
    }
}

//{
//    "users": [
//        {
//        "username": "clown",
//            "password": "test"
//        }
//    ]
//}

// Bin\debug\app\here
//[
//    {
//        "username": "clown",
//       "password": "test"
//    }
//]
