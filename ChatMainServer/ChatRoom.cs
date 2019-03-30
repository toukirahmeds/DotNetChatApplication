using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Collections;


namespace ChatMainServer{
    public class ChatRoom{
        private ObjectId _id;
        private string name;
        private ArrayList connectedUsers;
        private ArrayList chatHistory;
        public ChatRoom(string name){
            this._id = ObjectId.GenerateNewId();
            this.name = name;
            this.connectedUsers = new ArrayList();
            this.chatHistory = new ArrayList();
            
        }

        public ObjectId Id{
            get { return this._id;}
            set { this._id = value;}
        }

        public string Name  {
            get {return this.name;}
            set { this.name = value; }
        }


        public bool AddUser(User user){
            this.connectedUsers.Add( new ChatUser( user ) );
            return true;
        }

        
        public bool AddChatMessage( ObjectId userId, string message, string messageType ){
            this.chatHistory.Add( new ChatMessage( userId, message, messageType) );
            return true;
        }



        public void PrintUserList(){
            foreach(ChatUser user in this.connectedUsers) Console.WriteLine(user.Username);         
        }

        public ArrayList UserList{
            get { return this.connectedUsers;}
        }


        public ArrayList ChatHistory{
            get { return this.chatHistory;}
            set { this.chatHistory = value;}
        }


        public ArrayList ConnectedUsers{
            get { return this.connectedUsers;}
            set { this.connectedUsers = value;}
        }

        public bool HasUser(string username){
            foreach(ChatUser user in this.connectedUsers){
                if(user.Username == username) return true;
            }
            return false;
        }
    }
}