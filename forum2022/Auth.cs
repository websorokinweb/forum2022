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

        public static User GetCurrentUser()
        {
            Console.WriteLine(currentUser.username);
            Console.WriteLine(currentUser.password);
            return currentUser;
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

        public static bool CheckIfUserExist(User data)
        {
            User needItem = users.Find(x => x.username == data.username);
            return needItem.username != null;
        }

        public static void LoginUser(User data)
        {
            bool userInDb = CheckIfUserExist(data);
            if (userInDb)
            {
                Console.WriteLine("Successfully logged!");
                SetCurrentUser(data);
            }
            else
            {
                Console.WriteLine("FAIL! User with this username doesn't exist");
                // RegisterUser(data);
            }
        }

        public static void RegisterUser(User data)
        {
            bool userInDb = CheckIfUserExist(data);
            if (userInDb)
            {
                Console.WriteLine("User already registered!");
            }
            else
            {
                users.Add(data);

                // Saving file
                File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));
                Console.WriteLine("User successfully registered!");
            }
               

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
            testLogin.username = "eeee";
            testLogin.password = "test";
            LoginUser(testLogin);

            RegisterUser(testLogin);
        }
    }
}
