using System;

namespace ChatMainServer{
    public class UserAuthenticationException : Exception{
        public UserAuthenticationException(string message) : base(message){}
    }

    public class ChatRoomException :Exception{
        public ChatRoomException(string  message) : base(message){}
    }
}