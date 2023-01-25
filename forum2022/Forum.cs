using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace forum2022
{
    public struct Post{
        public string title { get; set; } 
        public string text { get; set; } 
        public string author { get; set; } 

    }

    internal class Forum
    {
        static string feedDbPath = "db/feed.json";

        static List<Post> posts = new List<Post>();

        public static void LoadPosts(){
            if (File.Exists(feedDbPath))
            {
                using (StreamReader jsonFile = new StreamReader(feedDbPath))
                {
                    string json = jsonFile.ReadToEnd();
                    posts = JsonSerializer.Deserialize<List<Post>>(json);
                }
            }
        }

        static List<Post> myPosts = new List<Post>();
        public static void LoadUserPosts(){
            myPosts = posts.FindAll(item => item.author == Auth.currentUser.username);
        }

        public static void DisplayPost(Post post){
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(post.title);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(post.text);
            Console.WriteLine("@" + post.author);
        }

        public static void ShowFeed(){
            Console.WriteLine("");
            for (int i = 0; i < posts.Count; i++){
                DisplayPost(posts[i]);
                if(i < posts.Count - 1){
                    Console.WriteLine("");
                }
            }
        }

        public static void DisplayMyPosts(){
            Console.WriteLine("");
            int postsAmont = 0;
            for (int i = 0; i < myPosts.Count; i++){
                if(myPosts[i].author == Auth.currentUser.username){
                    DisplayPost(myPosts[i]);
                    postsAmont++;
                    if(i < myPosts.Count - 1){
                        Console.WriteLine("");
                    }
                }
            }
            if(postsAmont != 0){
                Console.WriteLine("");
                Console.WriteLine("Wyniki: " + postsAmont);
            }else{
                Console.WriteLine("Jeszcze nie masz postów");
            }
        }

        public static void AddPost(string postTitle, string postText){
            Post newPost = new Post(){title = postTitle, text = postText, author = Auth.currentUser.username};
            posts.Add(newPost);
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));

            Console.WriteLine("Sukces! Post został opublikowany");
        }

        public static void DeletePost(Post post){

        }

        // Validation
        public static bool validatePostTitle(string userInput){
            if(userInput.Length >= 5){
                if(userInput.Length <= 60){
                    return true;            
                }
                Console.WriteLine("Tytuł nie może mieć więcej niż 60 symbolów");
                return false;
            }else{
                Console.WriteLine("Tytuł powinnen mieć minimum 5 symbolów");
                return false;
            }
        }
        public static bool validatePostText(string userInput){
            if(userInput.Length >= 15){
                if(userInput.Length <= 300){
                    return true;            
                }
                Console.WriteLine("Tekst nie może mieć więcej niż 300 symbolów");
                return false;
            }else{
                Console.WriteLine("Tekst powinnen mieć minimum 15 symbolów");
                return false;
            }
        }

        // Screens
        static string postTitle, postText;
        public static void CreatePostScreen(){
            postTitle = "";
            postText = "";
            Console.WriteLine("Tworzenia postu:");
            postTitle = Helpers.SaveUserStr("Tytuł:", validatePostTitle);
            postText = Helpers.SaveUserStr("Tekst:", validatePostText);
            AddPost(postTitle, postText);

            Menu.BackMenu(Auth.LoggedMenuScreen);
        }

        public static void ShowFeedScreen(){
            LoadPosts();
            ShowFeed();

            Menu.BackMenu(Auth.LoggedMenuScreen);
        }

        public static void ShowMyPostsScreen(){
            LoadPosts();
            LoadUserPosts();
            DisplayMyPosts();

            Menu.BackMenu(Auth.LoggedMenuScreen);
        }
    }
}