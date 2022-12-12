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
            string test = System.IO.Directory.GetCurrentDirectory();

            string startupPath = Environment.CurrentDirectory;
            string testLol = Environment.CurrentDirectory;

            Console.WriteLine(startupPath);
            Console.WriteLine(new FileInfo(startupPath).DirectoryName);

            // if (File.Exists("~/db/test.json"))
            /*using (StreamReader jsonFile = new StreamReader("~/db/test.json"))
            {
                string json = jsonFile.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(json);
            }*/
            //}
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
                RegisterUser(data);
            }
        }

        public static void RegisterUser(User data)
        {
            users.Add(data);
            Console.WriteLine(users[0]);
            Console.WriteLine(users[1]);

            // Check if it save file
            File.WriteAllText("test.json", JsonSerializer.Serialize(users));
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
            testLogin.username = "clon";
            testLogin.password = "test";
            LoginUser(testLogin);
        }
    }
}
