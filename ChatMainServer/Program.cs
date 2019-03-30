using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatMainServer
{

    class Program
    {


        static void CreateUserSeed(){
            try{
                // User u1 = UserController.CreateUser("toukir","123456");
                // Console.WriteLine("User created successfully");
                // Authentication.SignIn("toukir","123456");

                // User u1 = UserController.CreateUser("ahmed","123456");
                // Console.WriteLine("User created successfully");
                // Authentication.SignIn("ahmed","123456");

                User u1 = UserController.CreateUser("shuvo","123456");
                Console.WriteLine("User created successfully");
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

        
        
        
        static void Main(string[] args)
        {
            Configs.SetConfigs();
            // CreateUserSeed();
            // CreateChatRoomSeed();
            User u1 =  UserController.GetUserUsingUsername("toukir");
            ChatRoom c1 = ChatRoomController.GetChatRoomUsingName("Room 1");
            // c1.AddChatRoomUser(u1);
            // u1.SendMessage(c1, "Hello Everyone");
            // u1.DownloadChatHistory();
            u1.SearchChatMessages("ever");
            // S3BucketController.S3CreateBucket();

        }
    }
}
