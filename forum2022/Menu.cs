using System;
using System.Collections.Generic;
using System.Text;

namespace forum2022
{

    public struct MenuOption{
        public string title { get; set; }
        public string onChooseFunc { get; set; }
    }

    public class Menu
    {
        public static void CallFunctionByName(string funcName){
            switch(funcName){
                // Auth screens
                case "LoginScreen":
                    Auth.LoginScreen();
                    break;
                case "RegisterScreen":
                    Auth.RegisterScreen();
                    break;

                case "GetAllUsers":
                    Auth.GetAllUsers();
                    break;
                case "GetCurrentUser":
                    Auth.GetCurrentUser();
                    break;
                case "ShowProfile":
                    Auth.ShowProfile();
                    break;
            }
        }

        static List<MenuOption> currentMenuOptions = new List<MenuOption>();
        // {
                // new MenuOption(){title = "Get all users", onChooseFunc = "GetAllUsers"},
                // new MenuOption(){title = "Get current user", onChooseFunc = "GetCurrentUser"},
                // new MenuOption(){title = "Show my profile", onChooseFunc = "ShowProfile"}
            // }
        static int currentMenuOption = 0;
        static string additionalMenuMessage = "";

        public static void setAdditionalMenuMessage(string message){
            additionalMenuMessage = message;
        }

        public static void SetMenuOptions(){
            
        }

        public static void SetCategoryMenuOptions(string screen){
            currentMenuOptions = new List<MenuOption>();
            switch(screen){
                case "PickLoginOrRegister":
                    currentMenuOptions = new List<MenuOption>(){
                        new MenuOption(){title = "Login", onChooseFunc = "LoginScreen"},
                        new MenuOption(){title = "Register", onChooseFunc = "RegisterScreen"},
                    };
                    break;
                case "Logged":
                    currentMenuOptions = new List<MenuOption>(){
                        new MenuOption(){title = "Show My Profile", onChooseFunc = "ShowProfile"},
                        new MenuOption(){title = "Show Profile", onChooseFunc = "ShowProfile"},
                    };
                    break;
            }
        }

        public static void ShowMenuOptions(){
            for (int i = 0; i < currentMenuOptions.Count; i++) 
            {
              Console.WriteLine((currentMenuOption == i ? "- " : "  ") + currentMenuOptions[i].title);
            }
        }

        public static void ShowMenu()
        {
            Console.CursorVisible = false;

            bool isListening = true;
            while (isListening)
            {
                Console.Clear();

                if (additionalMenuMessage != ""){
                    Console.WriteLine(additionalMenuMessage);
                }

                Console.WriteLine("Choose option using up/down keys. Enter for submit");
                ShowMenuOptions();
                
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch(keyPressed.Key){
                    case ConsoleKey.UpArrow:
                        if(currentMenuOption > 0){
                            currentMenuOption--;
                        }else{
                            currentMenuOption = currentMenuOptions.Count - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if(currentMenuOption < currentMenuOptions.Count - 1){
                            currentMenuOption++;
                        }else{
                            currentMenuOption = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.CursorVisible = true;
                        CallFunctionByName(currentMenuOptions[currentMenuOption].onChooseFunc);
                        isListening = false;
                        break;
                    case ConsoleKey.Escape:
                        isListening = false;
                        break;
                }
            }

            additionalMenuMessage = "";
            Console.CursorVisible = true;
        }
    }
}
