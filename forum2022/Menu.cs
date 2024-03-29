﻿using System;
using System.Collections.Generic;

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

                // Admin actions
                case "AdminActionsScreen":
                    AdminSpecialActions.AdminActionsScreen();
                    break;
                case "AdminCreateUser":
                    AdminSpecialActions.AdminCreateUser();
                    break;
                case "AdminUserListScreen":
                    AdminSpecialActions.AdminUserListScreen();
                    break;
                case "ClearPostsScreen":
                    AdminSpecialActions.ClearPostsScreen();
                    break;
                case "DeleteUserScreen":
                    AdminSpecialActions.DeleteUserScreen();
                    break;
                case "EditUserPasswordScreen":
                    AdminSpecialActions.EditUserPasswordScreen();
                    break;

                // Others
                case "ShowSomeoneProfile":
                    Auth.ShowSomeoneProfile();
                    break;

                // Logged actions
                case "ShowProfile":
                    Auth.ShowProfile();
                    break;
                case "GenderPickerScreen":
                    Auth.GenderPickerScreen();
                    break;
                case "LogOutUser":
                    Auth.LogOutUser();
                    break;

                // Edit profile
                case "UsernameEditScreen":
                    Auth.UsernameEditScreen();
                    break;
                case "PasswordEditScreen":
                    Auth.PasswordEditScreen();
                    break;
                case "NameSurnameEditScreen":
                    Auth.NameSurnameEditScreen();
                    break;
                case "setGenderMale":
                    Auth.setGender("male");
                    break;
                case "setGenderFemale":
                    Auth.setGender("female");
                    break;
                case "DatePickerScreen":
                    Auth.DatePickerScreen();
                    break;

                // Feed
                case "CreatePostScreen":
                    Forum.CreatePostScreen();
                    break;
                case "ShowFeedScreen":
                    Forum.ShowFeedScreen();
                    break;
                case "ShowMyPostsScreen":
                    Forum.ShowMyPostsScreen();
                    break;

                // Post
                case "DeletePost":
                    Forum.DeletePost();
                    break;
                case "LikePost":
                    Forum.LikePost();
                    break;
                case "DislikePost":
                    Forum.DislikePost();
                    break;
            }
        }

        static List<MenuOption> currentMenuOptions = new List<MenuOption>();
        static int currentMenuOption = 0;

        public static void SetMenuOptions(List<MenuOption> newOptions){
            currentMenuOptions = newOptions;
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
                        new MenuOption(){title = "Pokaż mój profil", onChooseFunc = "ShowProfile"},
                        new MenuOption(){title = "Idź do feedu foruma", onChooseFunc = "ShowFeedScreen"},
                        new MenuOption(){title = "Dodaj post", onChooseFunc = "CreatePostScreen"},
                        new MenuOption(){title = "Moje posty", onChooseFunc = "ShowMyPostsScreen"},
                        new MenuOption(){title = "Edytuj imię i nazwisko", onChooseFunc = "NameSurnameEditScreen"},
                        new MenuOption(){title = "Edytuj płeć", onChooseFunc = "GenderPickerScreen"},
                        new MenuOption(){title = "Edytuj datę urodzenia", onChooseFunc = "DatePickerScreen"},
                        new MenuOption(){title = "Zmień username", onChooseFunc = "UsernameEditScreen"},
                        new MenuOption(){title = "Zmień hasło", onChooseFunc = "PasswordEditScreen"},
                        new MenuOption(){title = "Wyloguj", onChooseFunc = "LogOutUser"},
                    };
                    if(Auth.currentUser.admin){
                        currentMenuOptions.Add(new MenuOption(){title = "Panel administratora", onChooseFunc = "AdminActionsScreen"});
                    }
                    break;
                case "AdminPanel":
                    currentMenuOptions = new List<MenuOption>(){
                        new MenuOption(){title = "Dodaj użytkownika", onChooseFunc = "AdminCreateUser"},
                        new MenuOption(){title = "Kontrola użytkowników", onChooseFunc = "AdminUserListScreen"},
                        new MenuOption(){title = "Wyczyść posty", onChooseFunc = "ClearPostsScreen"},
                    };
                    break;
                case "GenderPicker":
                    currentMenuOptions = new List<MenuOption>(){
                        new MenuOption(){title = "Mężczyzna", onChooseFunc = "setGenderMale"},
                        new MenuOption(){title = "Kobieta", onChooseFunc = "setGenderFemale"},
                    };
                    break;
                case "UserActionsForAdmin":
                    currentMenuOptions = new List<MenuOption>(){
                        new MenuOption(){title = "Usunąć konto", onChooseFunc = "DeleteUserScreen"},
                        new MenuOption(){title = "Zmień hasło", onChooseFunc = "EditUserPasswordScreen"},
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

        public static Action DefaultMenuMessage = () => Console.WriteLine("Menu");

        public static void ShowMenu(Action onEscFunction, Action PreMenuMessage)
        {
            Console.CursorVisible = false;
            currentMenuOption = 0;

            bool isListening = true;
            while (isListening)
            {
                Console.Clear();
                PreMenuMessage();

                Console.WriteLine("Wybierz opcję strzałkami w górę/w dół. Enter dla submitu. Esc żeby wrócić");
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
                        onEscFunction();
                        isListening = false;
                        break;
                }
            }
            Console.CursorVisible = true;
        }

        public static void ShowListAsMenu(List<Post> listVar, Action<Post, Boolean> DisplayListItem, Func<Post, bool> OnChooseFunction, Action onEscFunction, bool clearPreviosMessages = true)
        {
            Console.CursorVisible = false;
            currentMenuOption = 0;

            bool isListening = true;
            while (isListening)
            {
                if(clearPreviosMessages){
                    Console.Clear();
                }

                Console.WriteLine("Wybierz opcję strzałkami w górę/w dół. Enter dla submitu. Esc żeby wrócić");
                Console.WriteLine("");
                for(int i = 0; i < listVar.Count; i++){
                    bool isActive = i == currentMenuOption;
                    DisplayListItem(listVar[i], isActive);
                    if(i < listVar.Count - 1){
                        Console.WriteLine("");
                    }
                }
                
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch(keyPressed.Key){
                    case ConsoleKey.UpArrow:
                        if(currentMenuOption > 0){
                            currentMenuOption--;
                        }else{
                            currentMenuOption = listVar.Count - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if(currentMenuOption < listVar.Count - 1){
                            currentMenuOption++;
                        }else{
                            currentMenuOption = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.CursorVisible = true;
                        OnChooseFunction(listVar[currentMenuOption]);
                        isListening = false;
                        break;
                    case ConsoleKey.Escape:
                        onEscFunction();
                        isListening = false;
                        break;
                }
            }
            Console.CursorVisible = true;
        }

        public static void BackMenu(Action doAfterEscape){
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Nacisnij Esc żeby wrócić.");

            Console.CursorVisible = false;
            bool isListening = true;
            while (isListening){
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch(keyPressed.Key){
                    case ConsoleKey.Escape:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        doAfterEscape();
                        isListening = false;
                        break;
                }
            }
        }

        // Additional
        public static void ShowListAsMenuOfUsers(List<User> listVar, Action<User, Boolean> DisplayListItem, Func<User, bool> OnChooseFunction, Action onEscFunction, bool clearPreviosMessages = true)
        {
            Console.CursorVisible = false;
            currentMenuOption = 0;
            bool isListening = true;
            while (isListening)
            {
                if(clearPreviosMessages){
                    Console.Clear();
                }

                Console.WriteLine("Wybierz opcję strzałkami w górę/w dół. Enter dla submitu. Esc żeby wrócić");
                Console.WriteLine("");

                for(int i = 0; i < listVar.Count; i++){
                    bool isActive = i == currentMenuOption;
                    DisplayListItem(listVar[i], isActive);
                    if(i < listVar.Count - 1){
                        Console.WriteLine("");
                    }
                }
                
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch(keyPressed.Key){
                    case ConsoleKey.UpArrow:
                        if(currentMenuOption > 0){
                            currentMenuOption--;
                        }else{
                            currentMenuOption = listVar.Count - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if(currentMenuOption < listVar.Count - 1){
                            currentMenuOption++;
                        }else{
                            currentMenuOption = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.CursorVisible = true;
                        OnChooseFunction(listVar[currentMenuOption]);
                        isListening = false;
                        break;
                    case ConsoleKey.Escape:
                        onEscFunction();
                        isListening = false;
                        break;
                }
            }
            Console.CursorVisible = true;
        }
    }
}
