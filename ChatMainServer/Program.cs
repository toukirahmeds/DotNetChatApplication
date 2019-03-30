using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatMainServer
{

    class Program
    {
        

        static User currentLoggedInUser; 
        static ChatRoom selectedChatRoom;

        static void CreateUserSeed(){
            try{
                // User u1 = Authentication.SignUp("toukir","123456");
                // Console.WriteLine("User created successfully");
                // Authentication.SignIn("toukir","123456");

                // User u1 = Authentication.SignUp("ahmed","123456");
                // Console.WriteLine("User created successfully");
                // Authentication.SignIn("ahmed","123456");

                // User u1 = Authentication.SignUp("shuvo","123456");
                // Console.WriteLine("User created successfully");
                Authentication.SignIn("shuvo","123456");
                
            }catch(UserAuthenticationException e){
                Console.WriteLine("UserAuthenticationException Caught : {0}", e.Message);
            }
        }


        static void CreateChatRoomSeed(){
                try{
                    ChatRoom cr1 = ChatRoomController.CreateChatRoom("Room 1");
                }catch(ChatRoomException e){
                    Console.WriteLine("Chat Room Exception Caught : {0}", e.Message);
                }
        }


        static void BasicOperationsTest(){
            CreateUserSeed();
            // CreateChatRoomSeed();
            // User u1 =  UserController.GetUserUsingUsername("toukir");
            // ChatRoom c1 = ChatRoomController.GetChatRoomUsingName("Room 1");
            // c1.AddChatRoomUser(u1);
            // u1.SendMessage(c1, "Hello Everyone");
            // u1.DownloadChatHistory();
            // u1.SearchChatMessages("hel");
            // Authentication.OnlineUserList();
            // S3BucketController.S3CreateBucket();

            // Console.WriteLine( Utility.CheckHashMatch("123456", Utility.GetHash256String("123456")) );
        }


        static void SelectChatRoomScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS SELECT CHAT ROOM SCREEN");
            Console.WriteLine("Username : ");
            string Username = Console.ReadLine();
            Console.WriteLine("Password : ");
            string Password = Console.ReadLine();
            Authentication.SignUp(Username, Password);

            Console.WriteLine("================================");
        }

        static void ChatScreenMessageBlock(){
            Console.WriteLine("Please type your message below:");
            string chatText = Console.ReadLine();
            if(chatText.ToLower().CompareTo( "exit" ) == 0  ) SignedInUserScreen();
            else{
                currentLoggedInUser.SendMessage( selectedChatRoom, chatText );
                ChatScreenMessageBlock();
            }
        }


        static void ChatScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS CHAT SCREEN");
            Console.WriteLine("Type exit below to go to signed in user screen.");
            ChatScreenMessageBlock();

            Console.WriteLine("================================");
        }

        static void SearchInConversationsScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS Search in Conversation SCREEN");
            Console.WriteLine("Type your search Text below : ");
            string searchText = Console.ReadLine();
            currentLoggedInUser.SearchChatMessages(searchText);

            Console.WriteLine("================================");

            SignedInUserScreen();
        }


        static void SignedInUserScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS THE MAIN SCREEN");
            Console.WriteLine("Select one of the following to continue");
            Console.WriteLine("1) Go TO Chat Room Screen,    2) Search In Conversations  3) Download Full Chat History");
            Console.WriteLine("4) Add to Chat Room,    5) Sign Out");
            string CommandKey = Console.ReadLine();
            switch(CommandKey){
                case "1":
                    // ChatScreen();
                    selectedChatRoom = currentLoggedInUser.PrintAllConnectedChatRoomAndSelectChatRoom();
                    if(selectedChatRoom != null) ChatScreen();
                    break;
                case "2":
                    SearchInConversationsScreen();
                    break;
                case "3":
                    currentLoggedInUser.DownloadChatHistory();
                    break;
                case "4":
                    currentLoggedInUser.PrintAllChatRoomAndEnroll();
                    break;
                case "5":
                    currentLoggedInUser.SignOut();
                    MainScreen();
                    break;
                default:
                    break;
            }
            Console.WriteLine("================================");
            SignedInUserScreen();
        }


        static void SignUpScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS THE SIGN UP SCREEN");
            Console.WriteLine("Username : ");
            string Username = Console.ReadLine();
            Console.WriteLine("Password : ");
            string Password = Console.ReadLine();
            try{
                Authentication.SignUp(Username, Password);
            }catch(UserAuthenticationException e){
                Console.WriteLine("UserAuthenticationException : {0}", e.Message);
            }finally{
                Console.WriteLine("================================");
                MainScreen();
            }
        }

        static void SignInScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS THE SIGN In SCREEN");
            Console.WriteLine("Username : ");
            string Username = Console.ReadLine();
            Console.WriteLine("Password : ");
            string Password = Console.ReadLine();
            currentLoggedInUser = Authentication.SignIn(Username, Password);
            Console.WriteLine("================================");
            if(currentLoggedInUser == null) MainScreen(); 
            else SignedInUserScreen();

        }


        static void CreateChatRoomScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS THE Create Chat Room SCREEN");
            Console.WriteLine("Chat Room Name : ");
            string chatRoomName = Console.ReadLine();
            try{
                ChatRoomController.CreateChatRoom(chatRoomName);
            }catch(ChatRoomException e){
                Console.WriteLine("ChatRoomException Caught : {0}", e.Message);
            }finally{
                Console.WriteLine("================================");
            }
            

        }

        

        static void MainScreen(){
            Console.WriteLine("================================");
            Console.WriteLine("THIS IS THE MAIN SCREEN");
            Console.WriteLine("Select one of the following to continue");
            Console.WriteLine("1) Sign Up,    2) Sign In  3) Online User List 4) Create Chat Room");
            string CommandKey = Console.ReadLine();
            switch(CommandKey){
                case "1":
                    SignUpScreen();
                    break;
                case "2":
                    SignInScreen();
                    break;
                case "3":
                    Authentication.OnlineUserList();
                    break;
                case "4":
                    CreateChatRoomScreen();
                    break;
                default:
                    MainScreen();
                    break;

            }
            Console.WriteLine("================================");
            MainScreen();
        }

        
        
        
        static void Main(string[] args)
        {
            Configs.SetConfigs();
            MainScreen();
            // // S3BucketController.S3Upload("./DOCUMENTS/toukirahmeds/history.txt");
            // S3BucketController.S3GetObject();

        }
    }
}
