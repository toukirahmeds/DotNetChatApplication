using System;
using MongoDB.Bson;

namespace ChatMainServer{
    public class User{
        private ObjectId _id;
        private string username, password;
        private bool isLoggedIn = false;
        public User(string username, string password){
            this._id = ObjectId.GenerateNewId();
            this.username = username;
            this.password = Utility.GetHash256String(password);
            this.isLoggedIn = false;
        }

        public ObjectId Id{
            get { return this._id;}
            set { this._id = value;}
        }

        public string Username{
            get { return this.username;}
            set { this.username = value;}
        }

        public string Password{
            get { return this.password;}
            set { this.password = Utility.GetHash256String(value);}
        }

        public bool IsLoggedIn{
            get { return this.isLoggedIn;}
            set { this.isLoggedIn = value;}
        }

        public void Display(){
            Console.WriteLine("username : {0}, password : {1}", this.Username, this.Password);
        }
    }
}