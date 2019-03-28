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

        public ObjectId Id{
            get  {return this._id;}
        }

        public string Username{
            get { return this.username;}
        }
    }
}