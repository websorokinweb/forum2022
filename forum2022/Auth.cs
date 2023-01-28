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
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool admin { get; set; }
        public string nameSurname { get; set; }
        public string birthDate { get; set; }
        public string gender { get; set; }
    }

    public class Auth
    {
        // Db paths
        public static string usersDbPath = "db/users.json";


        public static User currentUser = new User();
        public static List<User> users;

        public static void SetCurrentUser(User data)
        {
            currentUser.id = data.id;
            currentUser.username = data.username;
            currentUser.password = data.password;
            currentUser.admin = data.admin;

            if (data.nameSurname != null)
            {
                currentUser.nameSurname = data.nameSurname;
            }
            if (data.birthDate != null)
            {
                currentUser.birthDate = data.birthDate;
            }
            if (data.gender != null)
            {
                currentUser.gender = data.gender;
            }
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
                data.id = users.Count + 1;
                bool isChecking = true;
                while (isChecking)
                {
                    int needIndex = users.FindIndex(item => item.id == data.id);
                    if(needIndex != -1)
                    {
                        data.id++;
                    }
                    else
                    {
                        isChecking = false;
                    }
                }
                users.Add(data);
                File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));
                return true;
            }
               

        }

        public static void LogOutUser()
        {
            currentUser = new User();
            PickLoginOrRegister();
        }

        public static void ShowSomeoneProfile(){
            User user = users.Find(x => x.id == Forum.pickedPost.author);
            Console.WriteLine("=====");
            if(user.nameSurname != null){
                Console.WriteLine(user.nameSurname);
            }else{
                Console.WriteLine("Imię i nazwisko: nie wypełniono");
            }
            Console.WriteLine("@" + user.username);
            if(user.birthDate != null){
                Console.WriteLine("Urodziny: " + user.birthDate);
            }else{
                Console.WriteLine("Urodziny: nie wypełniono");
            }
            if(user.gender != null){
                Console.WriteLine("Płeć: " + user.gender);
            }else{
                Console.WriteLine("Płeć: nie wypełniono");
            }
            Console.WriteLine("=====");
            Menu.BackMenu(() => Forum.PostScreen(Forum.pickedPost));
        }

        public static void ShowProfile()
        {
            Console.WriteLine("=====");
            if(currentUser.nameSurname != null){
                Console.WriteLine(currentUser.nameSurname);
            }else{
                Console.WriteLine("Nie podałeś swojego imienia i nazwiska");
            }
            Console.WriteLine("@" + currentUser.username);
            if(currentUser.birthDate != null){
                Console.WriteLine("Urodziny: " + currentUser.birthDate);
            }else{
                Console.WriteLine("Urodziny: nie wypełniono");
            }
            if(currentUser.gender != null){
                Console.WriteLine("Płeć: " + currentUser.gender);
            }else{
                Console.WriteLine("Płeć: nie wypełniono");
            }
            // Console.WriteLine(currentUser.hobby);
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
            GetAllUsers();
            PickLoginOrRegister();
        }

        // Setters
        public static void setNewPassword(string newPassword){
            int needUserIndex = users.FindIndex(item => item.id == currentUser.id);
            User needUser = users[needUserIndex];
            needUser.password = newPassword;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Nowe hasło zostało ustawione");
        }

        public static void setNewUsername(string newUsername){
            int needUserIndex = users.FindIndex(item => item.id == currentUser.id);
            User needUser = users[needUserIndex];
            needUser.username = newUsername;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Nowy login został ustawiony");
        }

        public static void SetNameSurname(string nameSurname){
            int needUserIndex = users.FindIndex(item => item.username == currentUser.username);
            User needUser = users[needUserIndex];
            needUser.nameSurname = nameSurname;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Imię i nazwisko zostało zmienione");
        }

        public static void setGender(string gender){
            if(gender == "male"){
                gender = "Mężczyzna";
            }else if(gender == "female"){
                gender = "Kobieta";
            }
            int needUserIndex = users.FindIndex(item => item.username == currentUser.username);
            User needUser = users[needUserIndex];
            needUser.gender = gender;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Obrana płeć - " + needUser.gender);
        }

        public static void SetBirthDate(string year, string month, string day){
            int needUserIndex = users.FindIndex(item => item.username == currentUser.username);
            User needUser = users[needUserIndex];
            needUser.birthDate = day + '.' + month + '.' + year;
            users[needUserIndex] = needUser;
            File.WriteAllText(usersDbPath, JsonSerializer.Serialize(users));

            GetAllUsers();
            SetCurrentUser(needUser);
            Console.WriteLine("Sukces! Data urodzenia została zmieniona");
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
            if(userInput.Length >= 20){
                Console.WriteLine("Login nie może mieć więcej niż 20 znaków");
                return false;
            }else if(userInput.Length < 5){
                Console.WriteLine("Login nie może mieć mniej niż 5 znaków");
                return false;
            }else{
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
        }

        public static bool validateRegisterPassword(string userInput)
        {
            if(userInput.Length >= 8 && userInput.Length <= 32){
                if(!userInput.Contains(" ")){
                    userToCheck.password = userInput;
                    RegisterUser(userToCheck);
                    Console.WriteLine("Sukces! Użytkownik jest zarejestorowany!");
                    LoginUser(userToCheck);
                    return true;
                }else{
                    Console.WriteLine("Pomyłka! Hasło nie może mieć spacji!");
                    return false;
                }
            }else{
                Console.WriteLine("Hasło musi mieć minimum 8 znaky");
                return false;
            }
        }

        public static bool validateNameSurname(string userInput){
            if(userInput.Length >= 3){
                return true;
            }else{
                Console.WriteLine("Imię musi mieć minimum 3 symbole!");
                return false;
            }
        }

        public static bool validateBirthDateYear(string userInput){
            string message = "Nieprawidlowy format zapisu. Wartość powinna być od 1940 do 2006";
            int correctUserInput;
            if(userInput.Length == 4){
                if(!int.TryParse(userInput, out correctUserInput)){
                    Console.WriteLine(message);
                    return false;
                }
                if(correctUserInput >= 1940 && correctUserInput <= 2006){
                    return true;
                }
            }
            Console.WriteLine(message);
            return false;
        }
        public static bool validateBirthDateMonth(string userInput){
            int correctUserInput;
            string message = "Nieprawidlowy format zapisu. Wartość powinna być od 01 do 12";
            if(userInput.Length == 2){
                if(!int.TryParse(userInput, out correctUserInput)){
                    Console.WriteLine(message);
                    return false;
                }
                if(correctUserInput >= 1 && correctUserInput <= 12){
                    return true;
                }
            }
            Console.WriteLine(message);
            return false;
        }
        public static bool validateBirthDateDay(string userInput){
            int correctUserInput;
            string message = "Nieprawidlowy format zapisu. Przykład - 09";
            if(userInput.Length == 2){
                if(!int.TryParse(userInput, out correctUserInput)){
                    Console.WriteLine(message);
                    return false;
                }
                int intYear, intMonth;
                int.TryParse(year, out intYear);
                int.TryParse(month, out intMonth);
                int maxDayInMonth = DateTime.DaysInMonth(intYear, intMonth);
                Console.WriteLine(maxDayInMonth);
                if(correctUserInput >= 1 && correctUserInput <= maxDayInMonth){
                    return true;
                }else{
                    Console.WriteLine("Nieprawidlowa wartość. W " + month + "." + year + " było " + maxDayInMonth + " dni");
                }
            }
            Console.WriteLine(message);
            return false;
        }
        public static bool validateCurrentPassword(string userInput){
            if(userInput == currentUser.password){
                return true;
            }else{
                Console.WriteLine("Nieprawidlowe hasło. Sprobuj jeszcze raz");
                return false;
            }
        }

        public static bool validateNewPassword(string userInput){
            if(userInput.Length >= 8 && userInput.Length <= 32){
                return true;
            }else{
                Console.WriteLine("Hasło musi mieć minimum 8 znaky i maxsymalnie 32");
                return false;
            }
        }

        public static bool validateNewUsername(string userInput){
            if(userInput == currentUser.username){
                Console.WriteLine("Pomyłka! Nowy login taki sam jak stary");
                return false;
            }

            if(userInput.Length >= 20){
                Console.WriteLine("Pomyłka! Login nie może mieć więcej niż 20 znaky");
                return false;
            }else if(userInput.Length < 5){
                Console.WriteLine("Pomyłka! Login nie może mieć mniej niż 5 znaky");
                return false;
            }else{
                return true;
            }
        }

        // Screens

        static User userToCheck = new User();

        public static void WelcomeMessage(){
            Console.WriteLine("Witam na Forum2022))");
            Console.WriteLine("Zaloguj lub zarejestruj się:");
        }

        public static void PickLoginOrRegister(){
            Menu.SetCategoryMenuOptions("PickLoginOrRegister");
            Menu.ShowMenu(PickLoginOrRegister, WelcomeMessage);
        }

        public static void LoggedMenuScreen(){
            Menu.SetCategoryMenuOptions("Logged");
            Menu.ShowMenu(LoggedMenuScreen, Menu.DefaultMenuMessage);
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

        public static void UsernameEditScreen(){
            string newUsername, password;
            password = Helpers.SaveUserStr("Twoje hasło:", validateCurrentPassword);
            newUsername = Helpers.SaveUserStr("Nowy login:", validateNewUsername);
            setNewUsername(newUsername);

            Menu.BackMenu(LoggedMenuScreen);
        }

        public static void PasswordEditScreen(){
            string password, newPassword;
            password = Helpers.SaveUserStr("Twoje hasło:", validateCurrentPassword);
            newPassword = Helpers.SaveUserStr("Nowe hasło:", validateNewPassword);
            setNewPassword(newPassword);

            Menu.BackMenu(LoggedMenuScreen);
        }

        public static void NameSurnameEditScreen(){
            string newNameUsername;
            newNameUsername = Helpers.SaveUserStr("Imię i nazwisko:", validateNameSurname);
            SetNameSurname(newNameUsername);

            Menu.BackMenu(LoggedMenuScreen);
        }

        public static void GenderPickerScreen(){
            Menu.SetCategoryMenuOptions("GenderPicker");
            Menu.ShowMenu(LoggedMenuScreen, Menu.DefaultMenuMessage);

            Menu.BackMenu(LoggedMenuScreen);
        }

        static string year, month, day;
        public static void DatePickerScreen(){
            year = "";
            month = "";
            day = "";
            Console.WriteLine("Urodziny:");
            year = Helpers.SaveUserStr("Rok (1999):", validateBirthDateYear);
            month = Helpers.SaveUserStr("Miesiąc (09):", validateBirthDateMonth);
            day = Helpers.SaveUserStr("Dzień (09):", validateBirthDateDay);
            SetBirthDate(year, month, day);

            Menu.BackMenu(LoggedMenuScreen);
        }
    }
}
