using System;
using MongoDB.Bson;


namespace ChatMainServer{
    public class ChatUser{
        private ObjectId _id;
        private string username;

        public ChatUser(User user){
            this._id = user.Id;
            this.username = user.Username;
        }

        public ChatUser(ObjectId id, string username){
            this._id = id;
            this.username = username;
        }

        public ObjectId Id{
            get  {return this._id;}
            set { this._id = value;}
        }

        public string Username{
            get { return this.username;}
            set { this.username = value;}
        }
    }
}