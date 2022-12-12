using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace forum2022
{
    public struct User
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Auth
    {
        // Db paths
        static string usersDbPath = "db/users.json";


        static User currentUser = new User();
        static List<User> users;

        public static void SetCurrentUser(User data)
        {
            currentUser.username = data.username;
            currentUser.password = data.password;
        }

        public static void GetCurrentUser()
        {
            Console.WriteLine(currentUser.username);
            Console.WriteLine(currentUser.password);
        }

        public static void GetAllUsers()
        {
            if (File.Exists(usersDbPath))
            {
                using (StreamReader jsonFile = new StreamReader(usersDbPath))
                {
                    Console.WriteLine(jsonFile);
                    string json = jsonFile.ReadToEnd();
                    users = JsonSerializer.Deserialize<List<User>>(json);
                }
            }

        }

        public static void LoginUser(User data)
        {
            User needItem = users.Find(x => x.username == data.username);
            if (needItem.username != null)
            {
                Console.WriteLine("User with this username exist");
                SetCurrentUser(needItem);
            }
            else
            {
                Console.WriteLine("FAIL! User with this username doesn't exist");
                // RegisterUser(data);
            }
        }

        public static void RegisterUser(User data)
        {
            users.Add(data);
            Console.WriteLine(users[0]);
            Console.WriteLine(users[1]);

            // Check if it save file
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));
            Console.WriteLine("User successfully registered!");
        }

        public static void LogOutUser()
        {
            currentUser.username = "";
            currentUser.password = "";
        }

        public static void InitAuth()
        {
            Console.WriteLine("Auth module init");
            GetCurrentUser();
            GetAllUsers();

            User testLogin = new User();
            testLogin.username = "test";
            testLogin.password = "test";
            LoginUser(testLogin);

            RegisterUser(testLogin);
        }
    }
}
