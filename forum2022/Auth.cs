using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace forum2022
{
    struct User
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Auth
    {
        static User currentUser = new User();
        static List<User> users;

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

        public static void ShowAllUsers()
        {
            if (File.Exists("~/db/test.json"))
                using (StreamReader jsonFile = new StreamReader("~/db/test.json"))
                {
                    string json = jsonFile.ReadToEnd();
                    users = JsonSerializer.Deserialize<List<User>>(json);
                }
            {
            }
        }
    }
}
