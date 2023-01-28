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
        public int author { get; set; } 

    }

    internal class Forum
    {
        static string feedDbPath = "db/feed.json";

        static List<Post> posts = new List<Post>();

        static string currentPostSection = "all_posts";
        public static Post pickedPost;

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
            myPosts = posts.FindAll(item => item.author == Auth.currentUser.id);
        }

        public static void DisplayPost(Post post, bool isActive){
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine((isActive ? "-" : " ") + " " + (post.title.Length > 35 ? post.title.Substring(0,35) + "..." : post.title));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  " + (post.text.Length > 80 ? post.text.Substring(0,80) + "..." : post.text));

            User user = Auth.users.Find(x => x.id == post.author);
            Console.WriteLine("  " + "@" + user.username);

            pickedPost = post;

            // .Substring(0,20)


            // Console.ForegroundColor = ConsoleColor.White;
            // Console.WriteLine(post.title);
            // Console.ForegroundColor = ConsoleColor.Gray;
            // Console.WriteLine(post.text);
            // Console.WriteLine("@" + post.author);
        }

        public static void ShowFeed(){
            Console.WriteLine("");
            for (int i = 0; i < posts.Count; i++){
                // DisplayPost(posts[i]);
                if(i < posts.Count - 1){
                    Console.WriteLine("");
                }
            }
        }

        public static void DisplayMyPosts(){
            Console.WriteLine("");
            int postsAmont = 0;
            for (int i = 0; i < myPosts.Count; i++){
                if(myPosts[i].author == Auth.currentUser.id){
                    // DisplayPost(myPosts[i]);
                    postsAmont++;
                    if(i < myPosts.Count - 1){
                        Console.WriteLine("");
                    }
                }
            }
            // if(postsAmont != 0){
            //     Console.WriteLine("");
            //     Console.WriteLine("Wyniki: " + postsAmont);
            // }else{
            //     Console.WriteLine("Jeszcze nie masz postów");
            // }
        }

        public static void AddPost(string postTitle, string postText){
            Post newPost = new Post(){title = postTitle, text = postText, author = Auth.currentUser.id};
            Console.WriteLine(posts.Count);
            posts.Add(newPost);
            Console.WriteLine(posts.Count);
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));

            Console.WriteLine("Sukces! Post został opublikowany");
        }

        public static void DeletePost(){
            posts.Remove(pickedPost);
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));
            Console.WriteLine("Sukces! Post został usunięty");
        }

        // Validation
        public static bool validatePostTitle(string userInput){
            if(userInput.Length >= 5){
                if(userInput.Length <= 80){
                    return true;            
                }
                Console.WriteLine("Tytuł nie może mieć więcej niż 80 symbolów");
                return false;
            }else{
                Console.WriteLine("Tytuł powinnen mieć minimum 5 symbolów");
                return false;
            }
        }
        public static bool validatePostText(string userInput){
            if(userInput.Length >= 15){
                if(userInput.Length <= 400){
                    return true;            
                }
                Console.WriteLine("Tekst nie może mieć więcej niż 400 symbolów");
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

        public static void PostScreenBody(Post item){
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(item.title);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(item.text);
            User user = Auth.users.Find(x => x.id == item.author);
            Console.WriteLine("@" + user.username);
            Console.WriteLine("");
        }

        public static bool PostScreen(Post item){
            pickedPost = item;
            List<MenuOption> currentPostOptions;
            if(item.author == Auth.currentUser.id){
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "Delete my post", onChooseFunc = "DeletePost"},
                };
            }else if(Auth.currentUser.admin){
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "View author", onChooseFunc = "ShowSomeoneProfile"},
                    new MenuOption(){title = "Delete post", onChooseFunc = "DeletePost"},
                };
            }else{
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "View author", onChooseFunc = "ShowSomeoneProfile"},
                };
            }

            Menu.SetMenuOptions(currentPostOptions);

            if(currentPostSection == "all_posts"){
                Menu.ShowMenu(ShowFeedScreen, () => PostScreenBody(item));
            }else if(currentPostSection == "my_posts"){
                Menu.ShowMenu(ShowMyPostsScreen, () => PostScreenBody(item));
            }

            // if(currentPostSection == "all_posts"){
            //     Menu.BackMenu(ShowFeedScreen);
            // }else if(currentPostSection == "my_posts"){
            //     Menu.BackMenu(ShowMyPostsScreen);
            // }
            return true;
        }

        public static void ShowFeedScreen(){
            LoadPosts();
            // ShowFeed();
            // DisplayPost
            currentPostSection = "all_posts";
            Menu.ShowListAsMenu(posts, DisplayPost, PostScreen, Auth.LoggedMenuScreen);

            Menu.BackMenu(ShowFeedScreen);
        }

        public static void ShowMyPostsScreen(){
            LoadPosts();
            LoadUserPosts();
            currentPostSection = "my_posts";
            Menu.ShowListAsMenu(myPosts, DisplayPost, PostScreen, Auth.LoggedMenuScreen);
            // if(postsAmont != 0){
            //     Console.WriteLine("");
            //     Console.WriteLine("Wyniki: " + postsAmont);
            // }else{
            //     Console.WriteLine("Jeszcze nie masz postów");
            // }

            Menu.BackMenu(ShowMyPostsScreen);
        }
    }
}