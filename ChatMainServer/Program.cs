using System;



namespace ChatMainServer
{

    class Program
    {

        
        
        static void Main(string[] args)
        {
            Configs.SetConfigs();
            // try{
            //     User u1 = UserController.CreateUser("toukir","123456");
            //     Console.WriteLine("User created successfully");
            //     Authentication.SignIn("toukir","123456");
                
            // }catch(UserAuthenticationException e){
            //     Console.WriteLine("UserAuthenticationException Caught : {0}", e.Message);
            // }finally{
            //    try{
            //         ChatRoom cr1 = ChatRoomController.CreateChatRoom("Room 1");
            //     }catch(ChatRoomException e){
            //         Console.WriteLine("Chat Room Exception Caught : {0}", e.Message);
            //     }finally{
                    
            //     }
            // }
            User u1 =  UserController.GetUserUsingUsername("toukir");
            ChatRoom c1 = ChatRoomController.GetChatRoomUsingName("Room 1");
            c1.AddChatRoomUser(u1);
            // c1.AddChatRoomUser(u1);
            
            
            
            

            // Authentication.OnlineUserList();
            
        }
    }
}
