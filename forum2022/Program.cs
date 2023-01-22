using System;

namespace forum2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Auth.InitAuth();
            Menu.InitMenu();
            // Menu.SetMenuOptions();
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
/*[
    {
        "username": "clown",
       "password": "test"
    }
]*/
