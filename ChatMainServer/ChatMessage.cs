using System;
using MongoDB.Bson;

namespace ChatMainServer{
    public class ChatMessage{
        
        private ObjectId userId;
        private string message;
        private DateTime messageTime;

        public ChatMessage(ObjectId userId, string message){
            this.userId = userId;
            this.message =  message;
        }

        public ObjectId UserId{
            get { return this.userId;}
            set { this.userId = value;}
        }

        public string Message{
            get { return this.message;}
        }

        public DateTime MessageTime{
            get { return this.messageTime;}
            set { this.messageTime = value;}
        }
    }
}