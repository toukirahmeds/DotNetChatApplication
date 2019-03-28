using System;


namespace ChatMainServer{
    public class ChatMessage{
        
        private string userId, message;
        private DateTime messageTime;

        public ChatMessage(string userId, string message){
           
            this.userId = userId;
            this.message =  message;
        }

        public string UserId{
            get { return this.userId;}
        }

        public string Message{
            get { return this.message;}
        }

        public DateTime MessageTime{
            get { return this.messageTime;}
        }
    }
}