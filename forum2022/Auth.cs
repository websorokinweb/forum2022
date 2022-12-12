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
        public string name { get; set; }
        public string surname { get; set; }
        public string birthDate { get; set; }
        public string gender { get; set; }
        public List<string> hobby { get; set; }
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

            if(data.name != null)
            {
                currentUser.name = data.name;
            }
            if (data.name != null)
            {
                currentUser.name = data.name;
            }
            if (data.surname != null)
            {
                currentUser.surname = data.surname;
            }
            if (data.birthDate != null)
            {
                currentUser.birthDate = data.birthDate;
            }
            if (data.gender != null)
            {
                currentUser.gender = data.gender;
            }
            if (data.hobby != null)
            {
                currentUser.hobby = data.hobby;
            }
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
                User needItem = users.Find(x => x.username == data.username);
                Console.WriteLine("Successfully logged!");
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

        public static void ShowProfile()
        {
            Console.WriteLine("=====");
            Console.WriteLine(currentUser.name + ' ' + currentUser.surname);
            Console.WriteLine("@" + currentUser.username);
            Console.WriteLine("Born: " + currentUser.birthDate);
            Console.WriteLine("Płeć: " + currentUser.gender);
            Console.WriteLine(currentUser.hobby);
            Console.WriteLine("=====");

        }

        public static void InitAuth()
        {
            Console.WriteLine("Auth module init");
            GetCurrentUser();
            GetAllUsers();

            User testLogin = new User();
            testLogin.username = "clown";
            testLogin.password = "test";
            LoginUser(testLogin);

            RegisterUser(testLogin);
            ShowProfile();
        }
    }
}
