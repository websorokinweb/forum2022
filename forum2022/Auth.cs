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
        public bool admin { get; set; }
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
            currentUser.admin = data.admin;

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
                    string json = jsonFile.ReadToEnd();
                    users = JsonSerializer.Deserialize<List<User>>(json);
                }
            }

        }

        public static bool CheckIfUserExist(string username)
        {
            User needItem = users.Find(x => x.username == username);
            return needItem.username != null;
        }

        public static bool LoginUser(User data)
        {
            bool userInDb = CheckIfUserExist(data.username);
            if (userInDb)
            {
                User needItem = users.Find(x => x.username == data.username);
                if(needItem.password == data.password){
                    Console.WriteLine("Sukces!");
                    SetCurrentUser(needItem);
                    return true;
                }else{
                    Console.WriteLine("Nieprawidlowe hasło lub login.");
                }
            }
            else
            {
                Console.WriteLine("FAIL! User with this username doesn't exist");
                // RegisterUser(data);
            }
            return false;
        }

        public static bool RegisterUser(User data)
        {
            bool userInDb = CheckIfUserExist(data.username);
            if (userInDb)
            {
                Console.WriteLine("User already registered!");
                return false;
            }
            else
            {
                users.Add(data);
                // Saving file
                File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));
                return true;
            }
               

        }

        public static void LogOutUser()
        {
            currentUser = new User();
            PickLoginOrRegister();
        }

        public static void ShowProfile()
        {
            Console.WriteLine("=====");
            Console.WriteLine(currentUser.name + ' ' + currentUser.surname);
            Console.WriteLine("@" + currentUser.username);
            Console.WriteLine("Born: " + currentUser.birthDate);
            Console.WriteLine("Płeć: " + currentUser.gender);
            Console.WriteLine(currentUser.hobby);
            if(currentUser.admin){
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Jesteś adminem!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.WriteLine("=====");

            Menu.BackMenu(LoggedMenuScreen);
        }

        public static void InitAuth()
        {
            GetCurrentUser();
            GetAllUsers();

            // User testLogin = new User();
            // testLogin.username = "clown";
            // testLogin.password = "test";
            // LoginUser(testLogin);

            // RegisterUser(testLogin);
            // ShowProfile();

            PickLoginOrRegister();
        }

        // Setters

        public static void setGender(string gender){
            int needUserIndex = users.FindIndex(item => item.username == currentUser.username);
            User needUser = users[needUserIndex];
            needUser.gender = gender;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Obrana płeć - " + needUser.gender);
        }

        // Validate
        public static bool validateLogin(string userInput){
            if(CheckIfUserExist(userInput)){
                return true;
            }else{
                Console.WriteLine("Użytkownika z takim loginem nie istnieje.");
                return false;
            }
        }

        public static bool validatePassword(string userInput){
            userToCheck.password = userInput;
            if(LoginUser(userToCheck)){
                return true;
            }else{
                return false;
            }
        }

        public static bool validateRegisterLogin(string userInput)
        {
            if (CheckIfUserExist(userInput))
            {
                Console.WriteLine("Użytkownik z takim loginem już istnieje.");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool validateRegisterPassword(string userInput)
        {
            userToCheck.password = userInput;
            if (RegisterUser(userToCheck))
            {
                Console.WriteLine("Sukces! Użytkownik jest zarejestorowany!");
                LoginUser(userToCheck);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Screens

        static User userToCheck = new User();

        public static void PickLoginOrRegister(){
            Menu.setAdditionalMenuMessage("Zaloguj lub zarejestruj się:");
            Menu.SetCategoryMenuOptions("PickLoginOrRegister");
            Menu.ShowMenu();
        }

        public static void LoggedMenuScreen(){
            Menu.SetCategoryMenuOptions("Logged");
            Menu.ShowMenu();
        }

        public static void LoginScreen(){
            userToCheck = new User();
            Console.WriteLine("Logowanie:");
            userToCheck.username = Helpers.SaveUserStr("Login:", validateLogin);
            userToCheck.password = Helpers.SaveUserStr("Hasło:", validatePassword);

            LoggedMenuScreen();
        }

        public static void RegisterScreen(){
            userToCheck = new User();
            Console.WriteLine("Rejestracja:");
            userToCheck.username = Helpers.SaveUserStr("Login:", validateRegisterLogin);
            userToCheck.password = Helpers.SaveUserStr("Hasło:", validateRegisterPassword);

            LoggedMenuScreen();
        }

        public static void GenderPickerScreen(){
            Menu.SetCategoryMenuOptions("GenderPicker");
            Menu.ShowMenu();

            Menu.BackMenu(LoggedMenuScreen);
        }
    }
}
