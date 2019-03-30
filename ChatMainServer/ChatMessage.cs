using System;
using MongoDB.Bson;

namespace ChatMainServer{
    public class ChatMessage{
        
        private ObjectId userId;
        private string message;
        private DateTime messageTime;

        private string messageType;

        public ChatMessage(ObjectId userId, string message, string messageType = "TEXT"){
            this.userId = userId;
            this.message =  message;
            this.messageTime = DateTime.Now;
            this.messageType = messageType;
        }

        public ObjectId UserId{
            get { return this.userId;}
            set { this.userId = value;}
        }

        public string Message{
            get { return this.message;}
            set { this.message = value;}
        }

        public DateTime MessageTime{
            get { return this.messageTime;}
            set { this.messageTime = value;}
        }

        public string MessageType{
            get { return this.messageType;}
            set { this.messageType = value;}
        }
    }
}