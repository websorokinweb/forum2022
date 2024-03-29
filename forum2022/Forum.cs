using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace forum2022
{
    public struct Post{
        public int id { get; set; } 
        public string title { get; set; } 
        public string text { get; set; } 
        public int author { get; set; } 
        public List<int> likes { get; set; }
        public List<int> dislikes { get; set; }
    }

    internal class Forum
    {
        public static string feedDbPath = "db/feed.json";

        public static List<Post> posts = new List<Post>();

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

        public static void PostsListItemScreen(Post post, bool isActive){
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine((isActive ? "-" : " ") + " " + (post.title.Length > 35 ? post.title.Substring(0,35) + "..." : post.title));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  " + (post.text.Length > 80 ? post.text.Substring(0,80) + "..." : post.text));

            User user = Auth.users.Find(x => x.id == post.author);
            if(user.username == null){
                Console.WriteLine("  " + "Konto autora zostało usunięto");
            }else{
                Console.WriteLine("  " + "@" + user.username);
            }

            Console.WriteLine("  " + "Likes:" + post.likes.Count);
            Console.WriteLine("  " + "Dislikes:" + post.dislikes.Count);

            pickedPost = post;
        }

        public static void AddPost(string postTitle, string postText){
            Post newPost = new Post(){
                title = postTitle,
                text = postText,
                author = Auth.currentUser.id,
                likes = new List<int>(),
                dislikes = new List<int>()
            };
            newPost.id = posts.Count + 1;
            bool isChecking = true;
            while (isChecking)
            {
                int needIndex = posts.FindIndex(item => item.id == newPost.id);
                if(needIndex != -1)
                {
                    newPost.id++;
                }
                else
                {
                    isChecking = false;
                }
            }

            posts.Add(newPost);
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));

            Console.WriteLine("Sukces! Post został opublikowany");
        }

        public static void LikePost(){
            bool alreadyLiked = pickedPost.likes.Contains(Auth.currentUser.id);
            if(alreadyLiked){
                pickedPost.likes.Remove(Auth.currentUser.id);
                Console.WriteLine("Sukces! Like został usunięty");
            }else{
                pickedPost.likes.Add(Auth.currentUser.id);
                Console.WriteLine("Sukces! Like dodany");
                bool alreadyDisliked = pickedPost.dislikes.Contains(Auth.currentUser.id);
                if(alreadyDisliked){
                    pickedPost.dislikes.Remove(Auth.currentUser.id);
                }
            }

            int needPostIndex = posts.FindIndex(item => item.id == pickedPost.id);
            posts[needPostIndex] = pickedPost;
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));

            Menu.BackMenu(() => PostScreen(pickedPost));
        }

        public static void DislikePost(){
            bool alreadyDisliked = pickedPost.dislikes.Contains(Auth.currentUser.id);
            if(alreadyDisliked){
                pickedPost.dislikes.Remove(Auth.currentUser.id);
                Console.WriteLine("Sukces! Dislike został usunięty");
            }else{
                pickedPost.dislikes.Add(Auth.currentUser.id);
                Console.WriteLine("Sukces! Dislike dodany");
                bool alreadyLiked = pickedPost.likes.Contains(Auth.currentUser.id);
                if(alreadyLiked){
                    pickedPost.likes.Remove(Auth.currentUser.id);
                }
            }

            int needPostIndex = posts.FindIndex(item => item.id == pickedPost.id);
            posts[needPostIndex] = pickedPost;
            File.WriteAllText(feedDbPath, JsonSerializer.Serialize(posts));

            Menu.BackMenu(() => PostScreen(pickedPost));
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
            if(user.username == null){
                Console.WriteLine("Konto autora zostało usunięto");
            }else{
                Console.WriteLine("@" + user.username);
            }
            Console.WriteLine("Likes:" + item.likes.Count);
            Console.WriteLine("Dislikes:" + item.dislikes.Count);
            Console.WriteLine("");
        }

        public static bool PostScreen(Post item){
            pickedPost = item;
            List<MenuOption> currentPostOptions;
            if(item.author == Auth.currentUser.id){
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "Dislike", onChooseFunc = "DislikePost"},
                    new MenuOption(){title = "Delete my post", onChooseFunc = "DeletePost"},
                };
            }else if(Auth.currentUser.admin){
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "Dislike", onChooseFunc = "DislikePost"},
                    new MenuOption(){title = "View author", onChooseFunc = "ShowSomeoneProfile"},
                    new MenuOption(){title = "Delete post", onChooseFunc = "DeletePost"},
                };
            }else{
                currentPostOptions = new List<MenuOption>(){
                    new MenuOption(){title = "Like", onChooseFunc = "LikePost"},
                    new MenuOption(){title = "Dislike", onChooseFunc = "DislikePost"},
                    new MenuOption(){title = "View author", onChooseFunc = "ShowSomeoneProfile"},
                };
            }

            Menu.SetMenuOptions(currentPostOptions);

            if(currentPostSection == "all_posts"){
                Menu.ShowMenu(ShowFeedScreen, () => PostScreenBody(item));
            }else if(currentPostSection == "my_posts"){
                Menu.ShowMenu(ShowMyPostsScreen, () => PostScreenBody(item));
            }

            return true;
        }

        public static void ShowFeedScreen(){
            LoadPosts();
            currentPostSection = "all_posts";
            Menu.ShowListAsMenu(posts, PostsListItemScreen, PostScreen, Auth.LoggedMenuScreen);

            Menu.BackMenu(ShowFeedScreen);
        }

        public static void ShowMyPostsScreen(){
            LoadPosts();
            LoadUserPosts();
            currentPostSection = "my_posts";
            Menu.ShowListAsMenu(myPosts, PostsListItemScreen, PostScreen, Auth.LoggedMenuScreen);

            Menu.BackMenu(ShowMyPostsScreen);
        }
    }
}