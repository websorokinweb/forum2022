using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace forum2022
{
    public class AdminSpecialActions{
        // Setters
        public static void ClearPosts(){
            Forum.posts = new List<Post>();
            File.WriteAllText(Forum.feedDbPath, JsonSerializer.Serialize(Forum.posts));
        }

        // Validate
        public static bool validateAdminRegisterPassword(string userInput){
            if(userInput.Length >= 8 && userInput.Length <= 32){
                userToCheck.password = userInput;
                return true;
            }else{
                Console.WriteLine("Hasło musi mieć minimum 8 znaky");
                return false;
            }
        }
        public static bool validateIsAdmin(string userinput){
            if(userinput.ToLower() == "y"){
                return true;
            }else if(userinput.ToLower() == "n"){
                return true;
            }else{
                Console.WriteLine("Pomyłka! Y - żeby podtwierdzić, n - nie");
                return false;
            }
        }

        // Screens
        public static void AdminActionsScreen(){
            Menu.SetCategoryMenuOptions("AdminPanel");
            Menu.ShowMenu(Auth.LoggedMenuScreen, Menu.DefaultMenuMessage);
        }

        static User userToCheck = new User();
        public static void AdminCreateUser(){
            userToCheck = new User();
            Console.WriteLine("Rejestracja nowego użytkownika lub administratora:");
            string isAdminString;
            
            userToCheck.username = Helpers.SaveUserStr("Login:", Auth.validateRegisterLogin);
            userToCheck.password = Helpers.SaveUserStr("Hasło:", validateAdminRegisterPassword);
            isAdminString = Helpers.SaveUserStr("Role admin (Y, n):", validateIsAdmin);

            if(isAdminString.ToLower() == "y"){
                userToCheck.admin = true;
            }else{
                userToCheck.admin = false;
            }

            Auth.RegisterUser(userToCheck);

            Menu.BackMenu(AdminActionsScreen);
        }
        public static void ClearPostsScreen(){
            ClearPosts();
            Console.WriteLine("Posty zostały wyczyszczone");
            Menu.BackMenu(AdminActionsScreen);
        }
        public static void AdminUserListScreen(){
            Menu.ShowListAsMenuOfUsers(Auth.users, UsersListItemScreen, UserScreen, AdminActionsScreen);
        }
        public static void UsersListItemScreen(User item, bool isActive){
            Console.WriteLine((isActive ? "-" : " ") + " " + "@" + item.username);
            Console.WriteLine("  " + "id: " + item.id);
        }
        public static bool UserScreen(User item){
            Console.WriteLine();
            Console.WriteLine("id: " + item.id);
            Console.WriteLine("@" + item.username);
            Console.WriteLine("Hasło: " + item.password);
            Console.WriteLine();

            Console.WriteLine("Dodatkowe dane:");
            if(item.nameSurname != null){
                Console.WriteLine(item.nameSurname);
            }else{
                Console.WriteLine("Imię i nazwisko: nie wypełniono");
            }
            if(item.birthDate != null){
                Console.WriteLine("Urodziny: " + item.birthDate);
            }else{
                Console.WriteLine("Urodziny: nie wypełniono");
            }
            if(item.gender != null){
                Console.WriteLine("Płeć: " + item.gender);
            }else{
                Console.WriteLine("Płeć: nie wypełniono");
            }
            Console.WriteLine();

            Menu.BackMenu(AdminUserListScreen);

            return true;
        }
    }
}