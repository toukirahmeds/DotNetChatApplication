using System;



namespace ChatMainServer
{

    class Program
    {

        
        
        static void Main(string[] args)
        {
            Configs.SetConfigs();
            try{
                UserController.CreateUser("toukir","123456");
                Console.WriteLine("User created successfully");
                Authentication.SignIn("toukir","123456");
            }catch(UserAuthenticationException e){
                Console.WriteLine("UserAuthenticationException Caught : {0}", e.Message);
            }finally{
                try{
                    ChatRoomController.CreateChatRoom("Room 1");
                }catch(ChatRoomException e){
                    Console.WriteLine("Chat Room Exception Caught : {0}", e.Message);
                }
            }
            
            
            
            

            // Authentication.OnlineUserList();
            
        }
    }
}
